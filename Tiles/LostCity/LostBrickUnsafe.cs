using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Tiles.LostCity
{
    public class LostBrickUnsafe : InfectiousTile
    {
        public override void SetDefaults()
        {
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(14, 9, 20));

            dustType = DustID.t_Granite;
            drop = ModContent.ItemType<Items.LostCity.Placeables.LostBrick>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 2f;
            minPick = 100;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanInfect(int type)
        {
            return type == ModContent.TileType<LostBrick>();
        }
    }
}