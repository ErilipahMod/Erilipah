using Erilipah.KeyItems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Erilipah.Tiles.LostCity
{
    public class LostChest : ModTile
    {
        private static bool opening = false;

        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileValue[Type] = 500;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            this.AddMapEntry(new Color(200, 100, 200), "Lost Chest");
            dustType = DustID.PurpleCrystalShard;
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Containers };
        }

        public override bool HasSmartInteract() => KeyItemManager.Get<LostKey>().Unlocked;

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

        public override bool NewRightClick(int i, int j)
        {
            LostKey lostKey = KeyItemManager.Get<LostKey>();
            if (!lostKey.Unlocked && !opening)
            {
                opening = true;
                i -= Main.tile[i, j].frameX / 18;
                j -= Main.tile[i, j].frameY / 18;
                lostKey.Unlock(new Point(i + 1, j + 1).ToWorldCoordinates(0, 0));
                return true;
            }
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            if (!KeyItemManager.Get<LostKey>().Unlocked && !opening)
            {
                Main.LocalPlayer.showItemIconText = "Lost Chest";
                Main.LocalPlayer.noThrow = 2;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = 0;
            if (KeyItemManager.Get<LostKey>().Unlocked)
            {
                frameYOffset = 2 * 38;
            }
            else if (opening)
            {
                frameYOffset = 1 * 38;
            }
            else
            {
                frameYOffset = 0;
            }
        }
    }
}