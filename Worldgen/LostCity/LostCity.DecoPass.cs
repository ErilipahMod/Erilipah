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

                foreach (var building in BiomeManager.Get<LostCity>().Buildings)
                {
                    for (int j = building.Area.Top; j <= building.Area.Bottom; j += building.FloorHeight)
                    {
                        for (int i = building.Area.Left + 1; i < building.Area.Right; i++)
                        {
                            IterateFloor(i, j, j == building.Area.Top, j == building.Area.Bottom);
                        }
                    }
                }
            }

            private void IterateFloor(int i, int j, bool isRoof, bool isFloor)
            {
                const float bannerChance = 0.05f;

                // fuck it I'm hardcoding this for now
                // TODO fix banners entirely
                if (WorldGen.genRand.Chance(bannerChance) && !isFloor)
                    WorldGen.Place2xX(i, j + 1, (ushort)TileType<CityBanner>());

                // TODO put a lost city chest for each lost city key item

            }
        }
    }
}
