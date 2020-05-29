using Erilipah.Tiles.Epicenter.Plants;
using Mono.CompilerServices.SymbolWriter;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.Epicenter
{
    public class PlantPass : IBiomeGenPass
    {
        public string GenerateAfter => "Dungeon";

        public float Weight => 300;

        public void Generate(GenerationProgress progress)
        {
            progress.Message = "Planting plants";

            GrowStalks();
            GrowShrooms();
        }

        private static void GrowShrooms()
        {
            Epicenter epicenter = BiomeManager.Get<Epicenter>();
            int shroomsRemaining = 200; 
            while (shroomsRemaining > 0)
            {
                int i = Main.rand.Next(epicenter.Area.Left, epicenter.Area.Right + 1);
                int j = Main.rand.Next(epicenter.Area.Top, epicenter.Area.Bottom + 1);

                if (GetInstance<ErilipahShroom>().GrowthConditions(i, j))
                {
                    shroomsRemaining--;
                    GetInstance<ErilipahShroom>().Grow(i, j);
                }
            }
        }

        private static void GrowStalks()
        {
            Epicenter epicenter = BiomeManager.Get<Epicenter>();
            int stalksRemaining = Main.rand.Next(5, 10);

            for (int i = epicenter.Area.Left; i < epicenter.Area.Right && stalksRemaining > 0; i++)
            {
                int j = WorldgenExtensions.GetHighest(i, epicenter.Area.Top, p => !WorldGen.TileEmpty(p.X, p.Y));

                if (GetInstance<CrystallineStalk>().GrowthConditions(i, j))
                {
                    stalksRemaining--;
                    GetInstance<CrystallineStalk>().Grow(i, j);
                    for (int k = 0; k < 30; k++)
                    {
                        GetInstance<CrystallineStalk>().RandomUpdate(i, j - k);
                    }
                    i++;
                }
            }
        }
    }
}