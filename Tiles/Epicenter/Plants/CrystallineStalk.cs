using Erilipah.Dusts;
using Erilipah.Items.Epicenter.Placeables;
using Erilipah.Packets.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Pdb;
using NetEasy;
using System;
using System.CodeDom;
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

            dustType = ModContent.DustType<CrystallineDust>();

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
            // Only update the stalk if there's no tiles above us (AKA we're at the top)
            // This keeps growth rate constant.
            if (WorldGen.InWorld(i, j, 2) && WorldGen.TileEmpty(i, j - 1))
            {
                GrowVertically(i, j);
                GrowHorizontally(i, j);
                if (!WorldGen.gen)
                {
                    ChainFx(i, j);
                }
            }
        }

        private void ChainFx(int i, int j)
        {
            // Make fancy vfx
            new SpawnCrystallineStalkDust(new Vector2(i + Main.rand.NextFloat(-0.25f, 1.25f), j) * 16).Send();

            // Recurse.
            if (j + 1 < Main.maxTilesY)
            {
                Tile below = Framing.GetTileSafely(i, j + 1);
                if (below.active() && below.type == Type)
                {
                    ChainFx(i, j + 1);
                }
            }
        }

        private void GrowHorizontally(int i, int j)
        {
            for (int n = i - 1; n <= i + 1; n++)
            for (int m = j - 1; m <= j + 1; m++)
            {
                if (GrowthConditions(n, m))
                {
                    Grow(n, m);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendTileSquare(-1, n, m, 1);
                    return;
                }
            }
        }

        private void GrowVertically(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Tile above = Framing.GetTileSafely(i, j - 1);

            if (tile.frameX < 3 * frameSize) // If we're on the body of the stalk...
            {
                if (Main.rand.NextBool(10) || TileExtensions.AnyTilesIn(i, i, j - 7, j - 1)) // Top the stalk if there's a roof or after a random age
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
                above.active(true);

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendTileSquare(-1, i, j - 1, 1);
            }
            else if (tile.frameY > 0) // Otherwise, we're at the top. Decrease frameY
            {
                above.type = Type;
                above.frameX = tile.frameX;
                above.frameY = (short)(tile.frameY - frameSize);
                above.active(true);

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendTileSquare(-1, i, j - 1, 1);
            }
        }

        public override bool PreSpontaneousGrowth(int i, int j)
        {
            return Main.rand.NextBool(10);
        }

        public override bool GrowthConditions(int i, int j)
        {
            return WorldGen.InWorld(i, j, 1) && TileObject.CanPlace(i, j - 1, Type, 0, 0, out _, true) && !TileExtensions.AnyTilesIn(i, i, j - 20, j - 1);
        }

        public override void Grow(int i, int j)
        {
            WorldGen.PlaceTile(i, j - 1, Type, true);
        }
    }
}
