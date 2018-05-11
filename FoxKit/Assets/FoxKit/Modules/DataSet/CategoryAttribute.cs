namespace FoxKit.Modules.DataSet
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class CategoryAttribute : Attribute
    {
        public string Category { get; }

        public NestedInspectorMode ShowNestedInspector { get; }
        
        public CategoryAttribute(string category, NestedInspectorMode showNestedInspector = NestedInspectorMode.DontDraw)
        {
            this.Category = category;
            this.ShowNestedInspector = showNestedInspector;
        }

        public enum NestedInspectorMode
        {
            DontDraw,
            Draw
        }
    }
}