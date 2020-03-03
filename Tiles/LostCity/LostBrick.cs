using Terraria.ID;

namespace Erilipah.Tiles.LostCity
{
    public class LostBrick : LostBrickUnsafe
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            mineResist = 2f;
            minPick = 0;
        }

        public override bool CanInfect(int type)
        {
            return type == TileID.GrayBrick || type == TileID.RedBrick;
        }
    }
}
