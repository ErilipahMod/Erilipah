using Erilipah.Runnables;
using Erilipah.KeyItems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Erilipah.Tiles.LostCity
{
	public class LostChest : ModTile
	{
		private static bool opened = false;

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

			this.AddMapEntry(new Color(200, 200, 200), "Lost Chest");
			dustType = DustID.Granite;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
		}

		public override bool HasSmartInteract() => KeyItemManager.Get<LostKey>().Unlocked;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool NewRightClick(int i, int j)
		{
			LostKey lostKey = KeyItemManager.Get<LostKey>();
			if (!lostKey.Unlocked && !opened)
			{
				opened = true;
				lostKey.Unlock();
				return true;
			}
			return false;
		}

		public override void MouseOver(int i, int j)
		{
			if (KeyItemManager.Get<LostKey>().Unlocked)
			{
				Main.LocalPlayer.showItemIconText = "Lost Chest";
				Main.LocalPlayer.noThrow = 2;
			}
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (KeyItemManager.Get<LostKey>().Unlocked)
			{
				frame = 2 * 18;
			}
			else if (opened && frameCounter > 15)
			{
				frame = 1 * 18;
			}
		}
	}
}
