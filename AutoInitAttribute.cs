using System;

namespace Erilipah
{
    [Flags]
    public enum InitHooks { Load = 1, Unload = 2 }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class AutoInitAttribute : Attribute
    {
        public readonly InitHooks hooks;
        public readonly Type assignTo;

        public AutoInitAttribute(InitHooks hooks)
        {
            this.hooks = hooks;
        }

        public AutoInitAttribute(InitHooks hooks, Type assignTo) : this(hooks)
        {
            this.assignTo = assignTo;
        }
    }
}