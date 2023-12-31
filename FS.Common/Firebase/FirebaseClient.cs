namespace  HM.Common.Firebase.Database
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    //using  HM.Common.Firebase.Database.Offline;
    using  HM.Common.Firebase.Database.Query;

    /// <summary>
    /// Firebase client which acts as an entry point to the online database.
    /// </summary>
    public class FirebaseClient
    {
        internal readonly FirebaseOptions Options;

        private readonly string baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseClient"/> class.
        /// </summary>
        /// <param name="baseUrl"> The base url. </param>
        /// <param name="offlineDatabaseFactory"> Offline database. </param>  
        public FirebaseClient(string baseUrl, FirebaseOptions options = null)
        {
            this.Options = options ?? new FirebaseOptions();

            this.baseUrl = baseUrl;

            if (!this.baseUrl.EndsWith("/"))
            {
                this.baseUrl += "/";
            }
        }

        /// <summary>
        /// Queries for a child of the data root.
        /// </summary>
        /// <param name="resourceName"> Name of the child. </param>
        /// <returns> <see cref="ChildQuery"/>. </returns>
        public ChildQuery Child(string resourceName)
        {
            return new ChildQuery(this, () => this.baseUrl + resourceName);
        }
    }
}
