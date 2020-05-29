using Erilipah.Tiles;
using Erilipah.UI;
using Erilipah.Walls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Specialized;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;

namespace Erilipah
{
    public static class UIExtensions
    {
        public static ModifyInterfaceDelegate GetModifyInterfaceDel(this UserInterface @interface, string aboveLayer, string name)
        {
            return layers =>
            {
                int layerIndex = layers.FindIndex(layer => layer.Name.Equals(aboveLayer));
                if (layerIndex != -1)
                {
                    layerIndex++;
                    layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                        name,
                        delegate
                        {
                            @interface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            };
        }
    }

    public static class MiscExtensions
    {
        public static bool Chance(this UnifiedRandom rand, float chance) => rand.NextFloat() < chance;

        public static int Area(this Rectangle rect) => rect.Width * rect.Height;

        public static int GetInt(this NameValueCollection collection, string name) => int.Parse(collection[name]);

        public static float GetFloat(this NameValueCollection collection, string name) => float.Parse(collection[name]);
    }

    public static class WorldgenExtensions
    {
        public static int GetHighest(int i, int j, Predicate<Point> stopCondition)
        {
            while (WorldGen.InWorld(i, j) && !stopCondition(new Point(i, j)))
            {
                j++;
            }
            return j;
        }

        public static int GetHighestInRange(int i1, int i2, int j, Predicate<Point> stopCondition)
        {
            if (i1 > i2)
            {
                (i1, i2) = (i2, i1);
            }
            return Enumerable.Range(i1, i2 - i1).Min(i => GetHighest(i, j, stopCondition));
        }

        public static int GetLowestInRange(int i1, int i2, int j, Predicate<Point> stopCondition)
        {
            if (i1 > i2)
            {
                (i1, i2) = (i2, i1);
            }
            return Enumerable.Range(i1, i2 - i1).Max(i => GetHighest(i, j, stopCondition));
        }
    }

    public static class TileExtensions
    {
        public static bool AnyTilesIn(int startX, int endX, int startY, int endY)
        {
            if (!WorldGen.InWorld(startX, startY) || !WorldGen.InWorld(endX, endY))
            {
                return true;
            }
            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    if (Framing.GetTileSafely(i, j).active())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void Infect(int i, int j)
        {
            bool tileSuccess = InfectiousTile.InfectTile(i, j);
            bool wallSuccess = InfectiousWall.InfectWall(i, j);
            if (!WorldGen.gen && tileSuccess && wallSuccess)
            {
                WorldGen.SquareTileFrame(i, j);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, i, j, 3, TileChangeType.None);
                }
            }
        }

        public static void AddMapEntry(this ModTile tile, Color color, string name)
        {
            ModTranslation translation = tile.CreateMapEntryName();
            translation.SetDefault(name);
            tile.AddMapEntry(color, translation);
        }

        public static bool PlaceMultitile(int left, int top, int type, int sizeX, int sizeY, bool forced = false, int frameXOffset = 0, int frameYOffset = 0, int padding = 2)
        {
            int frameX = frameXOffset;
            if (!forced)
            {
                for (int i = left; i < left + sizeX; i++)
                    for (int j = top; j < top + sizeY; j++)
                        if (Framing.GetTileSafely(i, j).active())
                            return false;
            }
            for (int i = left; i < left + sizeX; i++)
            {
                int frameY = frameYOffset;
                for (int j = top; j < top + sizeY; j++)
                {
                    Tile tile = Main.tile[i, j];

                    tile.type = (ushort)type;
                    tile.frameX = (short)frameX;
                    tile.frameY = (short)frameY;
                    tile.active(true);
                    frameY += 16 + padding;
                }
                frameX += 16 + padding;
            }
            return true;
        }
    }
}