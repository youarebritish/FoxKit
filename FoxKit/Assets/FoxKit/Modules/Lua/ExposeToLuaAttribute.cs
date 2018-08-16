namespace FoxKit.Modules.Lua
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExposeClassToLuaAttribute : Attribute
    {
        
    }

    public enum MethodStaticity
    {
        Static,
        Instance
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExposeMethodToLuaAttribute : Attribute
    {
        public MethodStaticity Staticity { get; }

        public ExposeMethodToLuaAttribute(MethodStaticity staticity)
        {
            this.Staticity = staticity;
        }
    }
}