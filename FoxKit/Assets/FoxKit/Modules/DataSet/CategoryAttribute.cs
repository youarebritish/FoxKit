namespace FoxKit.Modules.DataSet
{
    using System;

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        public string Category { get; private set; }

        public CategoryAttribute(string category)
        {
            this.Category = category;
        }
    }
}