using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Tiles.Epicenter
{
    public class InfectedStone : InfectiousTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Stone[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedSoil>()] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedGlob>()] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(27, 25, 30));

            dustType = DustID.Stone;
            drop = ModContent.ItemType<Items.Epicenter.Placeables.InfectedSoil>();
            soundType = 21;
            soundStyle = 1;
            mineResist = 2f;
            minPick = 65;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanInfect(int type)
        {
            return TileID.Sets.Stone[type] || TileID.Sets.Conversion.Stone[type] || Main.tileStone[type] || type == TileID.Granite || type == TileID.Marble;
        }
    }
}
