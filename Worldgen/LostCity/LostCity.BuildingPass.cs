using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.World.Generation;

namespace Erilipah.Worldgen.LostCity
{
    public partial class LostCity
    {
        private class BuildingPass : IBiomeGenPass
        {
            public string GenerateAfter => "Altars";

            public float Weight => 350;

            private static LostCity Biome => BiomeManager.Get<LostCity>();

            private static void ErectBuildings(IEnumerable<LostBuilding> set)
            {
                LostBuilding lastBuilding = null;
                foreach (var building in set)
                {
                    building.ClearInside();
                    building.GenerateFrame();
                    building.GenerateFloors();
                    lastBuilding?.ConnectTo(building);
                    lastBuilding = building;
                }
            }

            private static IEnumerable<LostBuilding> InstantiateBuildings(int i, int j, int direction)
            {
                while (Math.Abs(i - Biome.Area.Center.X) < Biome.Area.Width / 2)
                {
                    int width = WorldGen.genRand.Next(16, 21);
                    int height = WorldGen.genRand.Next(Biome.Area.Height / 2, Biome.Area.Height);
                    int floorHeight = WorldGen.genRand.Next(10, 14);
                    height -= height % floorHeight;

                    int buildingJ = WorldgenExtensions.GetLowestInRange(i, i + width * direction, j, p => WorldGen.SolidOrSlopedTile(p.X, p.Y));
                    buildingJ += WorldGen.genRand.Next(-4, -1);

                    yield return new LostBuilding(direction, i, buildingJ, width, height, floorHeight);

                    i += direction * width + direction * WorldGen.genRand.Next(7, 13);
                }
            }

            public void Generate(GenerationProgress progress)
            {
                progress.Message = "Building up the Lost City";

                var leftSide = InstantiateBuildings(Biome.Area.Center.X - 10, Biome.Area.Bottom, -1);
                var rightSide = InstantiateBuildings(Biome.Area.Center.X + 10, Biome.Area.Bottom, +1);

                ErectBuildings(leftSide);
                ErectBuildings(rightSide);

                BiomeManager.Get<LostCity>().buildings = leftSide.Concat(rightSide).ToList();
            }
        }
    }
}
