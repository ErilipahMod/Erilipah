using Terraria.World.Generation;

namespace Erilipah.Worldgen
{
    public interface IBiomeGenPass
    {
        string GenerateAfter { get; }
        float Weight { get; }

        void Generate(GenerationProgress progress);
    }
}