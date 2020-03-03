using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Erilipah.Walls.Epicenter
{
    public class InfectedStoneWall : InfectiousWall
    {
        public override bool CanInfect(int type)
        {
            return WallID.Sets.Conversion.Stone[type];
        }

        public override void SetDefaults()
        {
            dustType = DustID.t_Granite;

            AddMapEntry(new Color(38, 38, 42));
        }
    }
}
