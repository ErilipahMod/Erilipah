using Erilipah.Core;
using Microsoft.Xna.Framework;
using System.Configuration;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.World.Generation;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Epicenter
    {
        private class Pass : IBiomeGenPass
        {
            public string GenerateAfter => "Dungeon";
            public float Weight => 700;

            public void Generate(GenerationProgress progress)
            {
                progress.Message = "Greater Erilipah";

                SetArea(progress);
                InfectArea(progress);
            }

            private static void SetArea(GenerationProgress progress)
            {
                var xPercent = ConfigReader.Get<float>("worldgen.epicenter.world edge offset");

                var dungeonLeft = Main.dungeonX < Main.maxTilesX / 2;

                var width = (int)(Main.maxTilesX * 0.08);
                var height = (int)(Main.maxTilesY * 0.7);
                int x = (int)(!dungeonLeft
                    ? xPercent * Main.maxTilesX
                    : (1 - xPercent) * Main.maxTilesX - width
                    );
                int y = WorldgenExtensions.GetHighestInRange(x + width / 2 - SurfaceWidth / 2, x + width / 2 + SurfaceWidth / 2, (int)WorldGen.worldSurfaceLow, p => WorldGen.SolidOrSlopedTile(p.X, p.Y));

                BiomeManager.Get<Epicenter>().Area = new Rectangle(x, y, width, height);
                progress.Value = 0.1f;
            }

            private static void InfectArea(GenerationProgress progress)
            {
                var area = BiomeManager.Get<Epicenter>().Area;
                var widthL = SurfaceWidth / 2f; // The left side of the biome's width.
                var widthR = SurfaceWidth / 2f; // The right side of the biome's width.
                var j = area.Top; // The current iterator. We iterate from top to bottom via rows.

                // Just a shortcut function :P
                void InfectRow()
                {
                    for (int i = area.Center.X - (int)widthL; i < area.Center.X + (int)widthR; i++)
                    {
                        TileExtensions.Infect(i, j);
                    }
                }
                float GetJPercent() => (j - area.Top) / (float)area.Height;

                // Initial surface. Skinny.
                for (; GetJPercent() < 0.1 && widthL + widthR < area.Width; j++)
                {
                    widthL += Rand(-1, 1);
                    widthR += Rand(-1, 1);
                    InfectRow();
                }
                progress.Value = 0.3f;

                float inc = 1f; // How the shape of the biome should expand/
                // Expansion. The underground is a lot larger than the above ground.
                for (; GetJPercent() < 0.5 && widthL + widthR < area.Width; j++)
                {
                    inc += 0.01f;
                    widthL += Rand(-inc / 3, inc);
                    widthR += Rand(-inc / 3, inc);
                    InfectRow();
                }
                progress.Value = 0.5f;

                // The more jagged edges of the bottom of the biome's bottom
                inc = 0f;
                for (; GetJPercent() < 0.8; j++)
                {
                    inc += 0.02f;
                    widthL += Rand(-inc, inc);
                    widthR += Rand(-inc, inc);
                    InfectRow();
                }
                progress.Value = 0.7f;

                // Finally, close the bottom of the biome
                for (; GetJPercent() < 1 && widthL + widthR > 0 && inc > 0; j++)
                {
                    inc -= 0.065f;
                    widthL += Rand(-inc - 1, inc / 2);
                    widthR += Rand(-inc - 1, inc / 2);
                    InfectRow();
                }
                progress.Value = 0.9f;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static float Rand(float min, float max) => WorldGen.genRand.NextFloat(min, max); // Just a shortcut function, so it's inlined :P
        }
    }
}
