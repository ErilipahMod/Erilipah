using Erilipah.Core;
using Microsoft.Xna.Framework;
using Noise;
using System.Configuration;
using Terraria;
using Terraria.World.Generation;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Mouth
    {
        private class TunnelPass : IBiomeGenPass
        {
            public string GenerateAfter => "Slush";
            public float Weight => 300;

            public void Generate(GenerationProgress progress)
            {
                int tunnelsDepth = ConfigReader.Get<int>("worldgen.tunnels.maze depth");

                progress.Message = "Erilipah Tunnels";

                var noise = new FastNoise(WorldGen._genRandSeed)
                {
                    NoiseType = FastNoise.NoiseTypes.CubicFractal,
                    FractalType = FastNoise.FractalTypes.Billow,
                    Frequency = 0.08f
                };

                var basin = BiomeManager.Get<Mouth>().Basin;
                var tunnels = BiomeManager.Get<Mouth>().Tunnels = new Rectangle(basin.X - basin.Width / 2, basin.Bottom - 2, basin.Width * 2, tunnelsDepth);
                var focal = tunnels.Center.ToVector2();
                var maxMiddleDist = (tunnels.TopLeft() - focal).LengthSquared();

                for (int i = tunnels.Left; i < tunnels.Right; i++)
                {
                    for (int j = tunnels.Top; j < tunnels.Bottom; j++)
                    {
                        var middleDist = (new Vector2(i, j) - focal).LengthSquared() / maxMiddleDist;
                        var result = noise.GetNoise(i, j);
                        if (result < -0.50f * (1 + middleDist))
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
            }
        }
    }
}
