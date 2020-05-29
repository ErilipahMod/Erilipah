using Erilipah.Core;
using Erilipah.Tiles.LostCity;
using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.LostCity
{
    public partial class LostCity
    {
        private class DecoPass : IBiomeGenPass
        {
            public string GenerateAfter => "Jungle Chests";

            public float Weight => 250;

            public void Generate(GenerationProgress progress)
            {
                progress.Message = "Decorating the Lost City";

                var buildings = BiomeManager.Get<LostCity>().buildings;
                foreach (var building in buildings)
                {
                    for (int j = building.Area.Top; j <= building.Area.Bottom; j += building.FloorHeight)
                    {
                        for (int i = building.Area.Left + 1; i < building.Area.Right; i++)
                        {
                            IterateFloor(i, j, j == building.Area.Bottom);
                        }
                    }
                }

                GenLostChest(buildings);
            }

            private void GenLostChest(IList<LostBuilding> buildings)
            {
                LostBuilding lostChestBuilding = Main.rand.Next(buildings);
                for (int attempt = 0; attempt < 100; attempt++)
                {
                    int i = WorldGen.genRand.Next(lostChestBuilding.Area.Left + 1, lostChestBuilding.Area.Right);
                    int j = lostChestBuilding.Area.Bottom - WorldGen.genRand.Next(lostChestBuilding.Floors) * lostChestBuilding.FloorHeight;

                    if (!WorldGen.SolidTile(i, j) || !WorldGen.SolidTile(i - 1, j))
                    {
                        continue;
                    }

                    if (TileExtensions.PlaceMultitile(i - 1, j - 2, TileType<LostChest>(), 2, 2))
                    {
                        return;
                    }
                }
                GenLostChest(buildings);
            }

            private void IterateFloor(int i, int j, bool isFloor)
            {
                float bannerChance = ConfigReader.Get<float>("worldgen.lost city.banners per tile");

                if (!isFloor && Framing.GetTileSafely(i, j).active() && WorldGen.genRand.Chance(bannerChance))
                {
                    TileExtensions.PlaceMultitile(i, j + 1, TileType<CityBanner>(), 2, 3);
                }
            }
        }
    }
}