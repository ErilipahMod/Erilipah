using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Tiles.Epicenter
{
    public class InfectedSoil : InfectiousTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedStone>()] = true;
            Main.tileMerge[Type][ModContent.TileType<InfectedGlob>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(25, 23, 27));

            dustType = DustID.PurpleCrystalShard;
            drop = ModContent.ItemType<Items.Epicenter.Placeables.InfectedSoil>();
            mineResist = 2f;
            minPick = 0;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }

        public override bool CanInfect(int type)
        {
            return type == TileID.Dirt || type == TileID.ClayBlock || TileID.Sets.Mud[type] || TileID.Sets.Grass[type] || TileID.Sets.GrassSpecial[type] || TileID.Sets.Conversion.Grass[type];
        }
    }
}
