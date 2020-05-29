using Erilipah.Projectiles;
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
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Erilipah.Tiles.Epicenter.Plants
{
    public class ErilipahShroom : PlantTile
    {
        public override IEnumerable<int> GrowthTiles { get; } = new int[]
        {
            ModContent.TileType<InfectedStone>(), ModContent.TileType<InfectedSoil>(), ModContent.TileType<InfectedGlob>()
        };

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileCut[Type] = true;

            TileObjectData.newTile.StyleWrapLimit = 5;
            TileObjectData.newTile.FlattenAnchors = true;

            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                20
            };
            TileObjectData.newTile.CoordinateWidth = 20;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = Point16.Zero;

            TileObjectData.newTile.AnchorValidTiles = (int[])GrowthTiles;
            TileObjectData baseData = new TileObjectData(TileObjectData.newTile);

            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);

            TileObjectData.newAlternate.CopyFrom(baseData);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.addAlternate(0);

            TileObjectData.newAlternate.CopyFrom(baseData);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.addAlternate(5);

            TileObjectData.newAlternate.CopyFrom(baseData);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.addAlternate(5);

            TileObjectData.addTile(Type);

            dustType = DustID.PurpleCrystalShard;

            soundType = SoundID.Item;
            soundStyle = 50;

            ModTranslation modTranslation = CreateMapEntryName();
            modTranslation.SetDefault("Blackshroom");
            AddMapEntry(new Color(0.4f, 0.1f, 0.7f), modTranslation);
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (WorldGen.SolidTile(i, j + 1))
                spriteEffects = SpriteEffects.None;
            else if (WorldGen.SolidTile(i, j - 1))
                spriteEffects = SpriteEffects.FlipVertically;
            else if (WorldGen.SolidTile(i + 1, j))
                spriteEffects = SpriteEffects.None;
            else
                spriteEffects = SpriteEffects.FlipHorizontally;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            const int frameHeight = 22;

            Tile tile = Framing.GetTileSafely(i, j);

            // Update for vertical -> horizontal
            if (tile.frameY == 0 && WorldGen.TileEmpty(i, j - 1) && WorldGen.TileEmpty(i, j + 1))
            {
                tile.frameY = frameHeight;
            }

            // Update for horizontal -> vertical
            if (tile.frameY == frameHeight && WorldGen.TileEmpty(i - 1, j) && WorldGen.TileEmpty(i + 1, j))
            {
                tile.frameY = 0;
            }

            // It's annoying that Terraria doesn't do this shit automatically.

            return true;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            base.ModifyLight(i, j, ref r, ref g, ref b);

            r = 0.35f;
            b = 0.35f;
        }

        public override void RandomUpdate(int i, int j)
        {
            float speedY = -2;
            if (WorldGen.TileEmpty(i, j + 1))
            {
                speedY = 2;
            }
            Projectile.NewProjectile(i * 16, j * 16, 0, speedY, ModContent.ProjectileType<Spore>(), 0, 0);
        }

        public override void Grow(int i, int j)
        {
            if (WorldGen.TileEmpty(i, j - 1))
                j--;
            else if (WorldGen.TileEmpty(i, j + 1))
                j++;
            else if (WorldGen.TileEmpty(i - 1, j))
                i--;
            else if (WorldGen.TileEmpty(i + 1, j))
                i++;

            WorldGen.PlaceTile(i, j, Type, true, style: Main.rand.Next(5));
        }

        public override bool PreSpontaneousGrowth(int i, int j)
        {
            return Main.rand.NextBool(7);
        }

        public override bool GrowthConditions(int i, int j)
        {
            return
                WorldGen.TileEmpty(i - 1, j) ||
                WorldGen.TileEmpty(i + 1, j) ||
                WorldGen.TileEmpty(i, j - 1) ||
                WorldGen.TileEmpty(i, j + 1);
        }

        public override bool KillSound(int i, int j)
        {
            Main.PlaySound(SoundID.NPCHit, i * 16, j * 16, 1, 1, -0.2f);
            return false;
        }
    }
}
