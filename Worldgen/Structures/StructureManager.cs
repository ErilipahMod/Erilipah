using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Erilipah.Worldgen.Structures
{
    public static class StructureManager
    {
        [AutoInit(InitHooks.Load | InitHooks.Unload)]
        private static readonly Dictionary<Type, Structure> structures = new Dictionary<Type, Structure>();

        public static T Get<T>() where T : Structure => (T)structures[typeof(T)];

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            Erilipah.Instance.OnCrawlType += Instance_OnCrawlType;
        }

        private static void Instance_OnCrawlType(Type obj)
        {
            if (obj.IsSubclassOf(typeof(Structure)) && !obj.IsAbstract)
            {
                Structure instance = (Structure)Activator.CreateInstance(obj);
                Load(instance);
                structures.Add(obj, instance);
            }
        }

        private static void Load(Structure instance)
        {
            var tex = ModContent.GetTexture(instance.Texture);
            instance.Load(tex);
        }
    }
}