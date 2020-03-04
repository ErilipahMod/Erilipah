using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Erilipah
{
    public static class ReflectionHelper
    {
        [AutoInit(InitHooks.Load | InitHooks.Unload)]
        private static readonly Dictionary<(string, Type), MemberInfo> memberCache = new Dictionary<(string, Type), MemberInfo>();

        private static MemberInfo GetOrMake((string, Type) key, Func<MemberInfo> factory)
        {
            if (memberCache.TryGetValue(key, out MemberInfo val))
            {
                return val;
            }
            val = factory();
            if (val == null)
            {
                throw new ArgumentException($"The given value does not exist. ({key})");
            }
            memberCache.Add(key, val);
            return val;
        }

        public static FieldInfo Field(this Type t, string name)
            => (FieldInfo)GetOrMake((name, t), () => t.GetField(name, BindingFlags.Instance | BindingFlags.Public |  BindingFlags.NonPublic));

        public static FieldInfo SField(this Type t, string name)
            => (FieldInfo)GetOrMake((name, t), () => t.GetField(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        public static PropertyInfo Property(this Type t, string name)
            => (PropertyInfo)GetOrMake((name, t), () => t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

        public static PropertyInfo SProperty(this Type t, string name)
            => (PropertyInfo)GetOrMake((name, t), () => t.GetProperty(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        public static MethodInfo Method(this Type t, string name)
            => (MethodInfo)GetOrMake((name, t), () => t.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

        public static MethodInfo SMethod(this Type t, string name)
            => (MethodInfo)GetOrMake((name, t), () => t.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        public static TMember Member<TMember>(this Type t, string name, MemberTypes memberType, BindingFlags flags) where TMember : MemberInfo
            => (TMember)GetOrMake((name, t), () => t.GetMember(name, memberType, flags).Single());
    }
}
