using Erilipah.Core;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Tiles
{
    public class ErilipahGlobalTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (WorldGen.InWorld(i, j - 1, 0) && Framing.GetTileSafely(i, j - 1).type == ModContent.TileType<LostCity.LostChest>())
            {
                return false;
            }
            return true;
        }

        public override void RandomUpdate(int i, int j, int type)
        {
            if (PlantTile.Plants.TryGetValue(type, out IEnumerable<PlantTile> plants)) 
            {
                foreach (var plant in plants)
                {
                    if (plant.ShouldGrow(i, j))
                    {
                        plant.Grow(i, j);
                    }
                } 
            }
        }
    }
}
