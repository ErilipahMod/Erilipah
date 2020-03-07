using Erilipah.Core;
using Microsoft.Xna.Framework;
using System;
using System.Configuration;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Erilipah.Worldgen.LostCity
{
    public class LostBuilding
    {
        public Rectangle Area { get; }
        public int FloorHeight { get; }
        public int Floors => Area.Height / FloorHeight;

        public LostBuilding(int genDirection, int i, int baseJ, int width, int height, int floorHeight)
        {
            FloorHeight = floorHeight;
            Area = new Rectangle(
                genDirection == -1 ? i - width : i,
                baseJ - height,
                width,
                height
                );
        }

        public LostBuilding(Rectangle area, int floorHeight)
        {
            Area = area;
            FloorHeight = floorHeight;
        }

        public void GenerateFrame()
        {
            int lostBrick = ModContent.TileType<Tiles.LostCity.LostBrickUnsafe>();

            // Generate the outside and clear the inside

            // Cycle through every Y layer
            for (int j = Area.Top; j <= Area.Bottom; j++)
            {
                // Place the outer tile walls
                WorldGen.PlaceTile(Area.Left, j, lostBrick, forced: true);
                WorldGen.PlaceTile(Area.Left - 1, j, lostBrick, forced: true);

                WorldGen.PlaceTile(Area.Right, j, lostBrick, forced: true);
                WorldGen.PlaceTile(Area.Right + 1, j, lostBrick, forced: true);
            }
        }

        public void ClearInside()
        {
            for (int j = Area.Top + 1; j < Area.Bottom; j++)
            {
                // Clear out the inside
                if (j > Area.Top)
                    for (int i = Area.Left + 1; i <= Area.Right - 1; i++)
                    {
                        WorldGen.KillTile(i, j);

                        WorldGen.KillWall(i, j);
                        WorldGen.PlaceWall(i, j, ModContent.WallType<Walls.LostCity.LostBrickWall>());
                    }
            }
        }

        public void GenerateFloors()
        {
            // Cycle through each floor
            for (int j = Area.Top; j <= Area.Bottom; j += FloorHeight)
            {
                // Place the floor, excluding 3 blocks for a platform
                int platform = Main.rand.Next(Area.Left + 1, Area.Right - 3);
                for (int i = Area.Left + 1; i <= Area.Right - 1; i++)
                {
                    if (i == platform || i == platform + 1 || i == platform + 2)
                    {
                        continue;
                    }
                    WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.LostCity.LostBrickUnsafe>());
                }
            }
        }

        public void ConnectTo(LostBuilding other)
        {
            int bridgeHeight = ConfigReader.Get<int>("worldgen.lost city.house bridge height");

            int right = Math.Max(Area.Left, other.Area.Left);
            int left = Math.Min(Area.Right, other.Area.Right);

            int floorShared = Math.Max(Area.Bottom, other.Area.Bottom);
            int roofShared = Math.Min(Area.Top, other.Area.Top);

            int bridgeFloor = WorldGen.genRand.Next(roofShared + bridgeHeight, floorShared);

            for (int i = left; i <= right; i++)
            {
                WorldGen.PlaceTile(i, bridgeFloor, ModContent.TileType<Tiles.LostCity.LostBrickUnsafe>(), forced: true);
                WorldGen.PlaceTile(i, bridgeFloor - bridgeHeight, ModContent.TileType<Tiles.LostCity.LostBrickUnsafe>(), forced: true);

                for (int j = bridgeFloor - bridgeHeight + 1; j < bridgeFloor; j++)
                {
                    WorldGen.KillTile(i, j);

                    WorldGen.KillWall(i, j);
                    WorldGen.PlaceWall(i, j, ModContent.WallType<Walls.LostCity.LostBrickWall>());
                }
            }
        }

        private class LostBuildingSerializer : TagSerializer<LostBuilding, TagCompound>
        {
            public override TagCompound Serialize(LostBuilding value) => new TagCompound
            {
                ["area"] = value.Area,
                ["floors"] = value.Floors
            };

            public override LostBuilding Deserialize(TagCompound tag)
            {
                return new LostBuilding(tag.Get<Rectangle>("area"), tag.GetInt("floors"));
            }
        }
    }
}
