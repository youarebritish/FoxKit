namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    public partial class Bucket
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the BucketCollector.
        /// </summary>
        public BucketCollector Collector
        {
            get
            {
                return this.collector as BucketCollector;
            }

            set
            {
                this.collector = value;
            }
        }
    }
}