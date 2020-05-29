using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace Erilipah.KeyItems
{
    public static class KeyItemManager
    {
        [AutoInit(InitHooks.Load | InitHooks.Unload)]
        private static readonly Dictionary<Type, KeyItem> items = new Dictionary<Type, KeyItem>();

        public static T Get<T>() where T : KeyItem => (T)items[typeof(T)];

        public static KeyItem Get(Type type) => items[type];

        public static IEnumerable<KeyItem> GetAll() => items.Values;

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            Erilipah.Instance.OnCrawlType += CrawlType;
        }

        private static void CrawlType(Type obj)
        {
            if (obj.IsSubclassOf(typeof(KeyItem)) && !obj.IsAbstract)
            {
                KeyItem biome = (KeyItem)Activator.CreateInstance(obj);
                biome.OnInitialize();
                items.Add(obj, biome);
            }
        }

        internal static void Save(TagCompound compound)
        {
            foreach (var item in items.Values)
            {
                TagCompound itemCompound = new TagCompound();
                item.Save(itemCompound);
                compound.Add(item.GetType().FullName, itemCompound);
            }
        }

        internal static void Load(TagCompound compound)
        {
            foreach (var item in items.Values)
            {
                TagCompound itemCompound = compound.GetCompound(item.GetType().FullName);
                item.Load(itemCompound);
            }
        }
    }
}