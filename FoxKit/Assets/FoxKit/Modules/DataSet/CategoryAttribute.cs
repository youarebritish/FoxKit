namespace FoxKit.Modules.DataSet
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class CategoryAttribute : Attribute
    {
        public string Category { get; }

        public bool ShowNestedInspector { get; }
        
        public CategoryAttribute(string category, bool showNestedInspector = false)
        {
            this.Category = category;
            this.ShowNestedInspector = showNestedInspector;
        }
    }
}