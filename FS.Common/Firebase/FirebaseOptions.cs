﻿namespace  HM.Common.Firebase.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    //using  HM.Common.Firebase.Database.Offline;

    using Newtonsoft.Json;

    public class FirebaseOptions
    {
        public FirebaseOptions()
        {
            //this.OfflineDatabaseFactory = (t, s) => new Dictionary<string, OfflineEntry>();
            this.SubscriptionStreamReaderFactory = s => new StreamReader(s);
            this.JsonSerializerSettings = new JsonSerializerSettings();
        }

        ///// <summary>
        ///// Gets or sets the factory for Firebase offline database. Default is in-memory dictionary.
        ///// </summary>
        //public Func<Type, string, IDictionary<string, OfflineEntry>> OfflineDatabaseFactory
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets or sets the method for retrieving auth tokens. Default is null.
        /// </summary>
        public Func<Task<string>> AuthTokenAsyncFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory for <see cref="TextReader"/> used for reading online streams. Default is <see cref="StreamReader"/>.
        /// </summary>
        public Func<Stream, TextReader> SubscriptionStreamReaderFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the json serializer settings.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings
        {
            get;
            set;
        }
    }
}
