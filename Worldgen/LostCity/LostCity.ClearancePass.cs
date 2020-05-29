using Microsoft.Xna.Framework;
using Noise;
using Terraria;
using Terraria.World.Generation;

namespace Erilipah.Worldgen.LostCity
{
    public partial class LostCity
    {
        private class ClearancePass : IBiomeGenPass
        {
            public string GenerateAfter => "Pyramids";

            public float Weight => 500;

            public void Generate(GenerationProgress progress)
            {
                const int lostCityHeight = 175;

                progress.Message = "Clearing out the Lost City";

                Rectangle tunnels = BiomeManager.Get<Epicenter.Mouth>().Tunnels;
                Rectangle area = BiomeManager.Get<LostCity>().Area = new Rectangle(tunnels.X, tunnels.Bottom - 10, tunnels.Width, lostCityHeight);

                var noise = new FastNoise(WorldGen._genRandSeed)
                {
                    NoiseType = FastNoise.NoiseTypes.CubicFractal,
                    FractalType = FastNoise.FractalTypes.Billow,
                    Frequency = 0.08f
                };
                var focal = new Vector2(area.Center.X, (int)(area.Top + area.Width * 0.8));
                var maxMiddleDist = (area.TopLeft() - focal).LengthSquared();

                for (int i = area.Left; i < area.Right; i++)
                {
                    for (int j = area.Top; j < area.Bottom; j++)
                    {
                        var middleDist = (new Vector2(i, j) - focal).LengthSquared() / maxMiddleDist;
                        var result = noise.GetNoise(i, j);
                        if (result < -0.52f * (1 + middleDist))
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
            }
        }
    }
}