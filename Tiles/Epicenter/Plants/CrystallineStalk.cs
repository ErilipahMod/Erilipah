using Erilipah.Items.Epicenter.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Erilipah.Tiles.Epicenter.Plants
{
    public class CrystallineStalk : PlantTile
    {
        private const short frameSize = 18;

        public override IEnumerable<int> GrowthTiles { get; } = new int[] 
        { 
            ModContent.TileType<InfectedStone>(), ModContent.TileType<InfectedSoil>(), ModContent.TileType<InfectedGlob>()
        };

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = Point16.Zero;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorValidTiles = (int[])GrowthTiles;
            TileObjectData.newTile.AnchorAlternateTiles = new int[]
            {
                Type
            };
            TileObjectData.addTile(Type);

            dustType = DustID.PurpleCrystalShard;

            soundType = SoundID.Item;
            soundStyle = 50;

            ModTranslation modTranslation = CreateMapEntryName();
            modTranslation.SetDefault("Crystalline Stalk");
            AddMapEntry(new Color(0.7f, 0.1f, 0.7f), modTranslation);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            base.ModifyLight(i, j, ref r, ref g, ref b);

            r = 0.5f;
            b = 0.5f;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (WorldGen.InWorld(i, j - 1) && (!Framing.GetTileSafely(i, j - 1).active() || Main.tile[i, j - 1].type == Type))
            {
                GrowVertically(i, j, tile);
            }

            if (WorldGen.InWorld(i, j, 2) && Main.tile[i, j + 1].type != Type)
            {
                GrowHorizontally(i, j);
            }
        }

        private void GrowHorizontally(int i, int j)
        {
            int direction = Main.rand.NextBool().ToDirectionInt();
            int distanceX = Main.rand.Next(1, 4);
            int n = i + direction * distanceX;
            for (int m = j - 1; m < j + 1; m++)
            {
                if (GrowthConditions(n, m))
                {
                    Grow(n, m);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendTileSquare(-1, n, m, 1);
                    break;
                }
            }
        }

        private void GrowVertically(int i, int j, Tile tile)
        {
            Tile above = Main.tile[i, j - 1];

            if (tile.frameX < 3 * frameSize) // If we're on the body of the stalk...
            {
                if (above.type == Type)
                {
                    TileLoader.RandomUpdate(i, j - 1, Type);
                }
                else if (Main.rand.NextBool(10) || TileExtensions.AnyTilesIn(i, i, j - 7, j - 1)) // Top the stalk if there's a roof or after a random age
                {
                    above.frameX = (short)(Main.rand.Next(3, 6) * frameSize);
                    above.frameY = 4 * frameSize;
                }
                else // Otherwise, continue stalk.
                {
                    above.frameX = (short)(Main.rand.Next(0, 3) * frameSize);
                    above.frameY = (short)(Main.rand.Next(0, 4) * frameSize);
                }
                above.type = Type;
            }
            else if (tile.frameY > 0) // Otherwise, we're at the top. Decrease frameY til we're 
            {
                above.type = Type;
                above.frameX = tile.frameX;
                above.frameY = (short)(tile.frameY - frameSize);
            }
            above.active(true);

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, i, j - 1, 1);
        }

        public override bool ShouldGrow(int i, int j)
        {
            return GrowthConditions(i, j) && Main.rand.NextBool(10);
        }

        private static bool GrowthConditions(int i, int j)
        {
            return WorldGen.InWorld(i, j, 1) && !TileExtensions.AnyTilesIn(i, i, j - 7, j - 1);
        }

        public override void Grow(int i, int j)
        {
            WorldGen.PlaceTile(i, j - 1, Type);
        }
    }
}
