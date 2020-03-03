using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Tiles.Epicenter
{
    public class InfectedGlob : InfectiousTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedStone>()] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedSoil>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(30, 25, 30));

            dustType = DustID.PurpleCrystalShard;
            drop = ModContent.ItemType<Items.Epicenter.Placeables.InfectedGlob>();
            soundType = 3;
            soundStyle = 2;
            mineResist = 1.5f;
            minPick = 0;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }

        public override bool CanInfect(int type)
        {
            return false;
        }
    }
}
