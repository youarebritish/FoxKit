namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System.Collections.Generic;

    public partial class BucketCollector
    {
        /// <summary>
        /// Get the Buckets.
        /// </summary>
        public IDictionary<string, Bucket> Buckets => this.buckets;

        /// <summary>
        /// Gets or sets the main Bucket.
        /// </summary>
        public Bucket MainBucket
        {
            get
            {
                return this.mainBucket;
            }

            set
            {
                this.mainBucket = value;
            }
        }
    }
}