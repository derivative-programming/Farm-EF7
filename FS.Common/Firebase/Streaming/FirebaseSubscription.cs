namespace HM.Firebase.Database.Streaming
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using HM.Firebase.Database.Query;

    using Newtonsoft.Json.Linq;
    using System.Net;

    /// <summary>
    /// The firebase subscription.
    /// </summary>
    /// <typeparam name="T"> Type of object to be streaming back to the called. </typeparam>
    internal class FirebaseSubscription<T> : IDisposable
    {
        private readonly CancellationTokenSource cancel;
        private readonly IObserver<FirebaseEvent<T>> observer;
        private readonly IFirebaseQuery query;
        private readonly FirebaseCache<T> cache;
        private readonly string elementRoot;
        private readonly FirebaseClient client;

        private static HttpClient http;

        static FirebaseSubscription()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 10,
                CookieContainer = new CookieContainer()
            };

            var httpClient = new HttpClient(handler, true);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            http = httpClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseSubscription{T}"/> class.
        /// </summary>
        /// <param name="observer"> The observer.  </param>
        /// <param name="query"> The query.  </param>
        /// <param name="cache"> The cache. </param>
        public FirebaseSubscription(IObserver<FirebaseEvent<T>> observer, IFirebaseQuery query, string elementRoot, FirebaseCache<T> cache)
        {
            this.observer = observer;
            this.query = query;
            this.elementRoot = elementRoot;
            this.cancel = new CancellationTokenSource();
            this.cache = cache;
            this.client = query.Client;
        }

        public event EventHandler<ExceptionEventArgs<FirebaseException>> ExceptionThrown;

        public void Dispose()
        {
            this.cancel.Cancel();
        }

        public IDisposable Run()
        {
            Task.Run(() => this.ReceiveThread());

            return this;
        }

        private async void ReceiveThread()
        {
            while (true)
            {
                var url = string.Empty;
                var line = string.Empty;

                try
                {
                    this.cancel.Token.ThrowIfCancellationRequested();

                    // initialize network connection
                    url = await this.query.BuildUrlAsync().ConfigureAwait(false);
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    var serverEvent = FirebaseServerEventType.KeepAlive;

                    var client = this.GetHttpClient();
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, this.cancel.Token).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var reader = this.client.Options.SubscriptionStreamReaderFactory(stream))
                    {
                        while (true)
                        {
                            line = reader.ReadLine()?.Trim();

                            if (string.IsNullOrWhiteSpace(line))
                            {
                                await Task.Delay(2000).ConfigureAwait(false);
                                continue;
                            }

                            var tuple = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                            
                            switch (tuple[0].ToLower())
                            {
                                case "event":
                                    serverEvent = this.ParseServerEvent(serverEvent, tuple[1]);
                                    break;
                                case "data":
                                    this.ProcessServerData(serverEvent, tuple[1]);
                                    break;
                            }

                            if (serverEvent == FirebaseServerEventType.AuthRevoked)
                            {
                                // auth token no longer valid, reconnect
                                break;
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    this.ExceptionThrown?.Invoke(this, new ExceptionEventArgs<FirebaseException>(new FirebaseException(url, string.Empty, line, ex)));

                    await Task.Delay(2000).ConfigureAwait(false);
                }
            }
        }

        private FirebaseServerEventType ParseServerEvent(FirebaseServerEventType serverEvent, string eventName)
        {
            switch (eventName)
            {
                case "put":
                    serverEvent = FirebaseServerEventType.Put;
                    break;
                case "patch":
                    serverEvent = FirebaseServerEventType.Patch;
                    break;
                case "keep-alive":
                    serverEvent = FirebaseServerEventType.KeepAlive;
                    break;
                case "cancel":
                    serverEvent = FirebaseServerEventType.Cancel;
                    break;
                case "auth_revoked":
                    serverEvent = FirebaseServerEventType.AuthRevoked;
                    break;
            }

            return serverEvent;
        }

        private void ProcessServerData(FirebaseServerEventType serverEvent, string serverData)
        {
            switch (serverEvent)
            {
                case FirebaseServerEventType.Put:
                case FirebaseServerEventType.Patch:
                    var result = JObject.Parse(serverData);
                    var path = result["path"].ToString();
                    var data = result["data"].ToString();

                    if (path == "/" && data == string.Empty)
                    {
                        this.observer.OnNext(FirebaseEvent<T>.Empty(FirebaseEventSource.Online));
                        return;
                    }

                    var eventType = string.IsNullOrWhiteSpace(data) ? FirebaseEventType.Delete : FirebaseEventType.InsertOrUpdate;

                    var items = this.cache.PushData(this.elementRoot + path, data);

                    foreach (var i in items.ToList())
                    {
                        this.observer.OnNext(new FirebaseEvent<T>(i.Key, i.Object, eventType, FirebaseEventSource.Online));
                    }

                    break;
                case FirebaseServerEventType.KeepAlive:
                    break;
                case FirebaseServerEventType.Cancel:
                    this.observer.OnError(new Exception("cancel"));
                    this.Dispose();
                    throw new OperationCanceledException();
                case FirebaseServerEventType.AuthRevoked:
                    this.observer.OnError(new Exception("auth"));
                    this.Dispose();
                    throw new OperationCanceledException();
            }
        }

        private HttpClient GetHttpClient()
        {
            return http;
        }
    }
}
