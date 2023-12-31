namespace  HM.Common.Firebase.Database.Query
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    //using System.Reactive.Linq;
    using System.Threading.Tasks;

    using  HM.Common.Firebase.Database.Http;
    //using  HM.Common.Firebase.Database.Offline;
    //using  HM.Common.Firebase.Database.Streaming;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a firebase query. 
    /// </summary>
    public abstract class FirebaseQuery : IFirebaseQuery, IDisposable
    {
        protected readonly FirebaseQuery Parent;

        private HttpClient client;

        /// <summary> 
        /// Initializes a new instance of the <see cref="FirebaseQuery"/> class.
        /// </summary>
        /// <param name="parent"> The parent of this query. </param>
        /// <param name="client"> The owning client. </param>
        protected FirebaseQuery(FirebaseQuery parent, FirebaseClient client)
        {
            this.Client = client;
            this.Parent = parent;
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        public FirebaseClient Client
        {
            get;
            set;
        }

        ///// <summary>
        ///// Queries the firebase server once returning collection of items.
        ///// </summary>
        ///// <typeparam name="T"> Type of elements. </typeparam>
        ///// <returns> Collection of <see cref="FirebaseObject{T}"/> holding the entities returned by server. </returns>
        //public async Task<IReadOnlyCollection<FirebaseObject<T>>> OnceAsync<T>()
        //{
        //    var path = await this.BuildUrlAsync().ConfigureAwait(false);

        //    return await this.GetClient().GetObjectCollectionAsync<T>(path).ConfigureAwait(false);
        //}

        public async Task<string> GetJSONTree()
        {
            var path = await this.BuildUrlAsync().ConfigureAwait(false);

            return await this.GetClient().GetJSONArrayAsync(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Assumes given query is pointing to a single object of type <typeparamref name="T"/> and retrieves it.
        /// </summary>
        /// <typeparam name="T"> Type of elements. </typeparam>
        /// <returns> Single object of type <typeparamref name="T"/>. </returns>
        public async Task<T> OnceSingleAsync<T>()
        {
            var path = await this.BuildUrlAsync().ConfigureAwait(false);
            var responseData = string.Empty;

            try
            {
                var response = await this.GetClient().GetAsync(path).ConfigureAwait(false);
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<T>(responseData);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(path, string.Empty, responseData, ex);
            }
        }

        ///// <summary>
        ///// Starts observing this query watching for changes real time sent by the server.
        ///// </summary>
        ///// <typeparam name="T"> Type of elements. </typeparam>
        ///// <param name="elementRoot"> Optional custom root element of received json items. </param>
        ///// <returns> Observable stream of <see cref="FirebaseEvent{T}"/>. </returns>
        //public IObservable<FirebaseEvent<T>> AsObservable<T>(EventHandler<ExceptionEventArgs<FirebaseException>> exceptionHandler = null, string elementRoot = "")
        //{
        //    return Observable.Create<FirebaseEvent<T>>(observer =>
        //    {
        //        var sub = new FirebaseSubscription<T>(observer, this, elementRoot, new FirebaseCache<T>());
        //        sub.ExceptionThrown += exceptionHandler;
        //        return sub.Run();
        //    });
        //}

        /// <summary>
        /// Builds the actual URL of this query.
        /// </summary>
        /// <returns> The <see cref="string"/>. </returns>
        public async Task<string> BuildUrlAsync()
        {
            // if token factory is present on the parent then use it to generate auth token
            if (this.Client.Options.AuthTokenAsyncFactory != null)
            {
                var token = await this.Client.Options.AuthTokenAsyncFactory().ConfigureAwait(false);
                return this.WithAuth(token).BuildUrl(null);
            }

            return this.BuildUrl(null);
        }

        ///// <summary>
        ///// Posts given object to repository.
        ///// </summary>
        ///// <param name="obj"> The object. </param> 
        ///// <param name="generateKeyOffline"> Specifies whether the key should be generated offline instead of online. </param> 
        ///// <typeparam name="T"> Type of <see cref="obj"/> </typeparam>
        ///// <returns> Resulting firebase object with populated key. </returns>
        //public async Task<FirebaseObject<T>> PostAsync<T>(T obj, bool generateKeyOffline = true)
        //{
        //    // post generates a new key server-side, while put can be used with an already generated local key
        //    if (generateKeyOffline)
        //    {
        //        var key = FirebaseKeyGenerator.Next();
        //        await new ChildQuery(this, () => key, this.Client).PutAsync(obj).ConfigureAwait(false);

        //        return new FirebaseObject<T>(key, obj);
        //    }
        //    else
        //    {
        //        var c = this.GetClient();
        //        var data = await this.SendAsync(c, obj, HttpMethod.Post).ConfigureAwait(false);
        //        var result = JsonConvert.DeserializeObject<PostResult>(data);

        //        return new FirebaseObject<T>(result.Name, obj);
        //    }
        //}

        /// <summary>
        /// Posts given object to repository.
        /// </summary>
        /// <param name="obj"> The object. </param> 
        /// <param name="generateKeyOffline"> Specifies whether the key should be generated offline instead of online. </param> 
        /// <typeparam name="T"> Type of <see cref="obj"/> </typeparam>
        /// <returns> Resulting firebase object with populated key. </returns>
        public async Task<string> PostJSONObjectAsync(string objData)
        {

            var c = this.GetClient();
            var data = await this.SendJSONDataAsync(c, objData, HttpMethod.Post).ConfigureAwait(false);

            return data;

        }

        ///// <summary>
        ///// Patches data at given location instead of overwriting them. 
        ///// </summary> 
        ///// <param name="obj"> The object. </param>  
        ///// <typeparam name="T"> Type of <see cref="obj"/> </typeparam>
        ///// <returns> The <see cref="Task"/>. </returns>
        //public async Task PatchAsync<T>(T obj)
        //{
        //    var c = this.GetClient();

        //    await this.SendAsync(c, obj, new HttpMethod("PATCH")).ConfigureAwait(false);
        //}

        public async Task PatchJSONObjectAsync(string objData)
        {
            var c = this.GetClient();

            await this.SendJSONDataAsync(c, objData, new HttpMethod("PATCH")).ConfigureAwait(false);
        }

        ///// <summary>
        ///// Sets or overwrites data at given location. 
        ///// </summary> 
        ///// <param name="obj"> The object. </param>  
        ///// <typeparam name="T"> Type of <see cref="obj"/> </typeparam>
        ///// <returns> The <see cref="Task"/>. </returns>
        //public async Task PutAsync<T>(T obj)
        //{
        //    var c = this.GetClient();

        //    await this.SendAsync(c, obj, HttpMethod.Put).ConfigureAwait(false);
        //}

        public async Task PutJSONObjectAsync(string objData)
        {
            var c = this.GetClient();

            await this.SendJSONDataAsync(c, objData, HttpMethod.Put).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes data from given location.
        /// </summary>
        /// <returns> The <see cref="Task"/>. </returns>
        public async Task DeleteAsync()
        {
            var c = this.GetClient();
            var url = await this.BuildUrlAsync().ConfigureAwait(false);
            var responseData = string.Empty;

            try
            {
                var result = await c.DeleteAsync(url).ConfigureAwait(false);
                responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new FirebaseException(url, string.Empty, responseData, ex);
            }
        }

        /// <summary>
        /// Disposes this instance.  
        /// </summary>
        public void Dispose()
        {
            this.client.Dispose();
        }

        /// <summary>
        /// Build the url segment of this child.
        /// </summary>
        /// <param name="child"> The child of this query. </param>
        /// <returns> The <see cref="string"/>. </returns>
        protected abstract string BuildUrlSegment(FirebaseQuery child);

        private string BuildUrl(FirebaseQuery child)
        {
            var url = this.BuildUrlSegment(child);

            if (this.Parent != null)
            {
                url = this.Parent.BuildUrl(this) + url;
            }

            return url;
        }

        private HttpClient GetClient()
        {
            if (this.client == null)
            {
                this.client = new HttpClient();
            }

            return this.client;
        }

        //private async Task<string> SendAsync<T>(HttpClient client, T obj, HttpMethod method)
        //{
        //    var url = await this.BuildUrlAsync().ConfigureAwait(false);
        //    var requestData = JsonConvert.SerializeObject(obj, this.Client.Options.JsonSerializerSettings);
        //    var message = new HttpRequestMessage(method, url)
        //    {
        //        Content = new StringContent(requestData)
        //    };

        //    var responseData = string.Empty;

        //    try
        //    {
        //        var result = await client.SendAsync(message).ConfigureAwait(false);
        //        responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

        //        result.EnsureSuccessStatusCode();

        //        return responseData;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new FirebaseException(url, requestData, responseData, ex);
        //    }
        //}
        private async Task<string> SendJSONDataAsync(HttpClient client, string requestData, HttpMethod method)
        {
            var url = await this.BuildUrlAsync().ConfigureAwait(false);
            // var requestData = JsonConvert.SerializeObject(obj, this.Client.Options.JsonSerializerSettings);
            var message = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(requestData)
            };

            var responseData = string.Empty;

            try
            {
                var result = await client.SendAsync(message).ConfigureAwait(false);
                responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                result.EnsureSuccessStatusCode();

                return responseData;
            }
            catch (Exception ex)
            {
                throw new FirebaseException(url, requestData, responseData, ex);
            }
        }
    }
}
