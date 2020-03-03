using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Walls.LostCity
{
    public class LostBrickWall : InfectiousWall
    {
        public override bool CanInfect(int type)
        {
            return type == WallID.RedBrick || type == WallID.GrayBrick;
        }

        public override void SetDefaults()
        {
            dustType = DustID.t_Granite;
            drop = ModContent.ItemType<Items.LostCity.Placeables.LostBrickWall>();

            AddMapEntry(new Color(7, 4, 10));
        }
    }
}
