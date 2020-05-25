using Terraria.World.Generation;

namespace Erilipah.Worldgen.Epicenter
{
    public class PlantPass : IBiomeGenPass
    {
        public string GenerateAfter => "Dungeon";

        public float Weight => 300;

        public void Generate(GenerationProgress progress)
        {
            // TODO: add plants n shit after area is generated
        }
    }
}