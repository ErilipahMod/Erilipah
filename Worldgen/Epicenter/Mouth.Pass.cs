using Erilipah.Core;
using Erilipah.Walls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Mouth
    {
        private class MouthPass : IBiomeGenPass
        {
            private static Rectangle Area => BiomeManager.Get<Epicenter>().Area;

            public string GenerateAfter => "Corruption";
            public float Weight => 300;

            public void Generate(GenerationProgress progress)
            {
                progress.Message = "Erilipah Epicenter";

                var o_baseWidth = ConfigReader.Get<int>("worldgen.mouth.outer spike base width");
                var o_attenuation = ConfigReader.Get<float>("worldgen.mouth.outer spike attenuation");
                GenerateSpikePair(Area.Center.X, 60, o_baseWidth, o_attenuation);

                var i_baseWidth = ConfigReader.Get<int>("worldgen.mouth.inner spike base width");
                var i_attenuation = ConfigReader.Get<float>("worldgen.mouth.inner spike attenuation");
                GenerateSpikePair(Area.Center.X, 35, i_baseWidth, i_attenuation);

                int pitStart = WorldgenExtensions.GetLowestInRange(Area.Center.X - 10, Area.Center.X + 10, (int)WorldGen.worldSurfaceLow, p => WorldGen.SolidOrSlopedTile(p.X, p.Y));
                GeneratePit(Area.Center.X, pitStart - 6);
            }

            private static void GeneratePit(int i, int j)
            {
                int basinDepth = ConfigReader.Get<int>("worldgen.mouth.basin depth");

                float widthL = 3;
                float widthR = 3;
                float inc = 0;

                void ClearRow()
                {
                    int left = i - (int)widthL;
                    int right = i + (int)widthR;
                    for (int k = left; k <= right; k++)
                    {
                        WorldGen.KillTile(k, j);

                        if (!InfectiousWall.InfectWall(k, j))
                        {
                            Framing.GetTileSafely(k, j).wall = (ushort)InfectiousWall.Default;
                        }
                    }
                }
                float GetJPercent() => (j - Area.Top) / (float)Area.Height;

                // Create the opening
                for (; GetJPercent() < 0.15f && widthL + widthR < Epicenter.SurfaceWidth * 0.8; j++)
                {
                    inc += 0.07f;
                    widthL += WorldGen.genRand.NextFloat(-inc / 2, inc);
                    widthR += WorldGen.genRand.NextFloat(-inc / 2, inc);
                    ClearRow();
                }

                Rectangle basin = new Rectangle(i - (int)widthL, j, (int)(widthL + widthR), basinDepth);

                // Create the "stomach" area
                int oldJ = j;
                for (; j < oldJ + basinDepth; j++)
                {
                    widthL += WorldGen.genRand.NextFloat(-1, 1);
                    widthR += WorldGen.genRand.NextFloat(-1, 1);
                    ClearRow();
                }

                for (; widthL + widthR > 0 && inc > 0; j++)
                {
                    inc -= 0.015f;
                    widthL += WorldGen.genRand.NextFloat(-inc - 1, inc / 2);
                    widthR += WorldGen.genRand.NextFloat(-inc - 1, inc / 2);
                    ClearRow();
                }

                int lowest = WorldgenExtensions.GetLowestInRange(basin.Left, basin.Right, basin.Top, p => WorldGen.SolidOrSlopedTile(p.X, p.Y));
                basin.Height = lowest - basin.Top;

                BiomeManager.Get<Mouth>().Basin = basin;
            }

            private static void GenerateSpikePair(int i, int iOffset, int baseWidth, float attenuation)
            {
                GenerateSpike(i - iOffset, 1, baseWidth, attenuation);
                GenerateSpike(i + iOffset, -1, baseWidth, attenuation);
            }

            private static void GenerateSpike(int i, int direction, int baseWidth, float attenuation)
            {
                // Get the lowest Y value
                int j = WorldgenExtensions.GetLowestInRange(i, i + baseWidth * direction, Area.Top, p => WorldGen.SolidOrSlopedTile(p.X, p.Y));

                // Add a base to the spike
                for (int k = 0; k < baseWidth; k++)
                {
                    for (int l = j; l < j + 4; l++)
                    {
                        WorldGen.PlaceTile(i + k * direction, j, TileType<Tiles.Epicenter.InfectedStone>(), forced: true);
                    }
                }

                // Create the jagged spike formation
                while (baseWidth > 0)
                {
                    for (int k = 0; k < baseWidth; k++)
                    {
                        WorldGen.PlaceTile(i + k * direction, j, TileType<Tiles.Epicenter.InfectedStone>(), forced: true);
                    }

                    if (WorldGen.genRand.NextFloat() < attenuation)
                    {
                        baseWidth--;
                    }

                    if (WorldGen.genRand.NextFloat() < attenuation)
                    {
                        i += 1 * direction;
                    }
                    j--;
                }
            }
        }
    }
}