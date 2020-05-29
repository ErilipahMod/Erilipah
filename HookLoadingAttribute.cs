using System;

namespace Erilipah
{
    [Flags]
    public enum LoadHooks { Load = 1, Unload = 2, PostLoad = 4 }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class HookLoadingAttribute : Attribute
    {
        public readonly LoadHooks hooks;

        public HookLoadingAttribute(LoadHooks hooks)
        {
            this.hooks = hooks;
        }
    }
}