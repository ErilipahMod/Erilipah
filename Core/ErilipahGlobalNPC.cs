using Erilipah.Worldgen;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Core
{
    public partial class ErilipahGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.EditSpawnPool(pool, spawnInfo);
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
            }
        }

        public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY);
            }
        }
    }
}
