using Erilipah.Worldgen;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace Erilipah
{
    public class ErilipahWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            BiomeManager.HandleWorldGenTasks(tasks);
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            BiomeManager.HandleTileCounts(tileCounts);
        }

        public override void ResetNearbyTileEffects()
        {
            BiomeManager.HandleTileCountResets();
        }

        public override TagCompound Save()
        {
            TagCompound compound = new TagCompound();
            BiomeManager.Save(compound);
            return compound;
        }

        public override void Load(TagCompound tag)
        {
            BiomeManager.Load(tag);
        }
    }
}
