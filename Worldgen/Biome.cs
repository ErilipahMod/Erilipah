using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Erilipah.Worldgen
{
    public abstract class Biome
    {
        private readonly Dictionary<int, bool> biomeStatuses = new Dictionary<int, bool>();

        public virtual bool ValidBiomeConditions(Player player) => true;
        public virtual IEnumerable<int> BiomeTileTypes => Enumerable.Empty<int>();
        public virtual IEnumerable<IBiomeGenPass> BiomeGenPasses => Enumerable.Empty<IBiomeGenPass>();

        public virtual int TileCountThreshold => 80;

        public int TileCounts { get; internal set; }

        public virtual void OnInitialize() { }
        public virtual void OnUpdateVisuals() { }
        public virtual void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) { }
        public virtual void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) { }
        public virtual void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY) { }
        public virtual void ModifySunlight(ref Color tileColor, ref Color backgroundColor, float opacity) { }
        public virtual void ModifyMusic(ref int music, ref MusicPriority priority) { }
        public virtual void ModifyMusic(ref int music, ref MusicPriority priority) { }
        public virtual void ModifySunlight(ref Color tileColor, ref Color backgroundColor, float fade) { }
        public virtual void Save(TagCompound compound) { }
        public virtual void Load(TagCompound compound) { }

        public bool GetInBiome(Player player) => biomeStatuses.TryGetValue(player.whoAmI, out bool ret) ? ret : false;
        public bool SetInBiome(Player player, bool inBiome) => biomeStatuses[player.whoAmI] = inBiome;

        internal void UpdateBiome(Player player)
        {
            biomeStatuses[player.whoAmI] = TileCounts >= TileCountThreshold && ValidBiomeConditions(player);
        }
    }
}
