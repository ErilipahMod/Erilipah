using Erilipah.Core;
using Erilipah.Tiles.Epicenter;
using Erilipah.Tiles.LostCity;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Configuration;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Epicenter : Biome
    {
        public static int SurfaceWidth => ConfigReader.Get<int>("worldgen.epicenter.surface width");

        public Rectangle Area { get; private set; }

        public override IEnumerable<int> BiomeTileTypes => new[]
        {
            TileType<InfectedStone>(), TileType<LostBrick>()
        };

        public override IEnumerable<IBiomeGenPass> BiomeGenPasses => new[]
        {
            new Pass()
        };

        public override void Save(TagCompound compound)
        {
            compound[nameof(Area)] = Area;
        }

        public override void Load(TagCompound compound)
        {
            Area = compound.Get<Rectangle>(nameof(Area));
        }
    }
}
