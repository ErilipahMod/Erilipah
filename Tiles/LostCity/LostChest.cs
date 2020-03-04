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
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);

			this.AddMapEntry(new Color(200, 200, 200), "Lost Chest");
			dustType = DustID.Granite;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
		}

		public override bool HasSmartInteract() => true;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool NewRightClick(int i, int j)
		{
			// FINISH: store the KeyItem and unlock it here. call Unlock on the key instance. ez
			if ()
			{

			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.showItemIconText = "Lost Chest";
			Main.LocalPlayer.noThrow = 2;
		}
	}
}
