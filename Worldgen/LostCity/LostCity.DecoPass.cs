using Erilipah.Core;
using Erilipah.Tiles.LostCity;
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

                var lostChestBuilding = WorldGen.genRand.Next(buildings);
                GenLostChest(lostChestBuilding);
            }

            private void GenLostChest(LostBuilding lostChestBuilding)
            {
                ushort type = (ushort)TileType<LostChest>();
                while (true)
                {
                    int i = WorldGen.genRand.Next(lostChestBuilding.Area.Left + 1, lostChestBuilding.Area.Right);
                    int j = lostChestBuilding.Area.Bottom - WorldGen.genRand.Next(lostChestBuilding.Floors) * lostChestBuilding.FloorHeight;

                    WorldGen.Place2x2(i, j - 2, type, 0);
                    if (Framing.GetTileSafely(i, j - 2).active() && Main.tile[i, j - 2].type == type)
                    {
                        break;
                    }
                }
            }

            private void IterateFloor(int i, int j, bool isFloor)
            {
                float bannerChance = ConfigReader.Get<float>("worldgen.lost city.banners per tile");

                if (!isFloor && WorldGen.genRand.Chance(bannerChance))
                    WorldGen.Place2xX(i, j + 1, (ushort)TileType<CityBanner>());
            }
        }
    }
}
