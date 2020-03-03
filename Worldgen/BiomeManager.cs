using System;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace Erilipah.Worldgen
{
    public static class BiomeManager
    {
        [AutoInit(InitHooks.Load | InitHooks.Unload)]
        private static readonly Dictionary<Type, Biome> biomes = new Dictionary<Type, Biome>();

        public static T Get<T>() where T : Biome => (T)biomes[typeof(T)];

        public static IEnumerable<Biome> GetAll() => biomes.Values;

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            Erilipah.Instance.OnCrawlType += CrawlType;
        }

        private static void CrawlType(Type obj)
        {
            if (obj.IsSubclassOf(typeof(Biome)) && !obj.IsAbstract)
            {
                Biome biome = (Biome)Activator.CreateInstance(obj);
                biome.OnInitialize();
                biomes.Add(obj, biome);
            }
        }

        internal static void Save(TagCompound compound)
        {
            foreach (var biome in biomes.Values)
            {
                TagCompound biomeCompound = new TagCompound();
                biome.Save(biomeCompound);
                compound.Add(biome.GetType().FullName, biomeCompound);
            }
        }

        internal static void Load(TagCompound compound)
        {
            foreach (var biome in biomes.Values)
            {
                TagCompound biomeCompound = compound.GetCompound(biome.GetType().FullName);
                biome.Load(biomeCompound);
            }
        }

        internal static void HandleWorldGenTasks(List<GenPass> tasks)
        {
            foreach (var biome in biomes.Values)
            {
                foreach (var genPass in biome.BiomeGenPasses)
                {
                    int index = tasks.FindIndex(g => g.Name == genPass.GenerateAfter);
                    if (index != -1)
                    {
                        tasks.Insert(index + 1, new PassLegacy($"Erilipah: {biome.GetType()}: {genPass.GetType()}", genPass.Generate, genPass.Weight));
                    }
                }
            }
        }

        internal static void HandleTileCounts(int[] tileCounts)
        {
            foreach (var biome in biomes.Values)
            {
                foreach (var type in biome.BiomeTileTypes)
                {
                    biome.TileCounts += tileCounts[type];
                }
                biome.OnUpdateTileCounts();
            }
        }

        internal static void HandleTileCountResets()
        {
            foreach (var biome in biomes.Values)
            {
                biome.TileCounts = 0;
                biome.OnResetTileCounts();
            }
        }
    }
}
