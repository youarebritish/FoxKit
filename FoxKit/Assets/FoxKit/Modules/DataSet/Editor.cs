namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.Lua;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// Main class for interacting with FoxKit's DataSet functionality.
    /// </summary>
    [Serializable, ExposeClassToLua]
    public class Editor : Game
    {
        /// <summary>
        /// The singleton Editor instance.
        /// </summary>
        private static Editor instance;

        /// <summary>
        /// Stores editable Buckets.
        /// </summary>
        [OdinSerialize, NonSerialized,
         PropertyInfo(
             Core.PropertyInfoType.EntityPtr,
             0,
             1,
             Core.ContainerType.StaticArray,
             PropertyExport.Never,
             PropertyExport.Never,
             typeof(BucketCollector))]
        private readonly BucketCollector editableBucketCollector = new BucketCollector();

        /// <summary>
        /// Stores editable Buckets.
        /// </summary>
        [OdinSerialize, NonSerialized,
         PropertyInfo(
             Core.PropertyInfoType.EntityPtr,
             0,
             1,
             Core.ContainerType.StaticArray,
             PropertyExport.EditorOnly,
             PropertyExport.Never,
             typeof(BucketCollector))]
        private readonly EntitySelector entitySelector = new EntitySelector();
        
        /// <summary>
        /// Assign the global Editor instance.
        /// </summary>
        /// <param name="editor">The Editor instance.</param>
        public static void Setting(Editor editor)
        {
            Assert.IsNotNull(editor);

            if (instance != null)
            {
                Debug.Log("The Editor instance has already been assigned!");
                return;
            }

            instance = editor;
        }

        /// <summary>
        /// Get the global Editor instance.
        /// </summary>
        /// <returns>The Editor instance.</returns>
        public static Editor GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Create a new editable Bucket.
        /// </summary>
        /// <param name="bucketName">Name of the new editable Bucket.</param>
        /// <returns>The new editable Bucket.</returns>
        public Bucket CreateNewEditableBucket(string bucketName)
        {
            var bucket = new Bucket { Name = bucketName, Collector = this.editableBucketCollector };
            this.editableBucketCollector.Buckets.Add(bucketName, bucket);
            return bucket;
        }

        /// <summary>
        /// Set the current editable Bucket.
        /// </summary>
        /// <param name="bucket">The Bucket.</param>
        public void SetCurrentEditableBucket(Bucket bucket)
        {
            this.editableBucketCollector.MainBucket = bucket;
        }

        /// <summary>
        /// Get the current editable Bucket.
        /// </summary>
        /// <returns>The current editable Bucket.</returns>
        public Bucket GetCurrentEditableBucket()
        {
            return this.editableBucketCollector.MainBucket;
        }

        /// <summary>
        /// Get the list of current editable Buckets.
        /// </summary>
        /// <returns>The list of current editable Buckets.</returns>
        public IDictionary<string, Bucket> GetEditableBucketList()
        {
            return this.editableBucketCollector.Buckets;
        }

        /// <summary>
        /// No idea what this does. Some kind of initialization.
        /// </summary>
        public void Setup()
        {
        }
    }
}