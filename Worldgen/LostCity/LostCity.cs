using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.LostCity
{
    public partial class LostCity : Biome
    {
        public Rectangle Area { get; private set; }

        private IList<LostBuilding> buildings;

        public override IEnumerable<int> BiomeTileTypes => new[]
        {
            TileType<Tiles.LostCity.LostBrick>(), TileType<Tiles.LostCity.LostBrickUnsafe>(),
        };

        public override IEnumerable<IBiomeGenPass> BiomeGenPasses => new IBiomeGenPass[]
        {
            new ClearancePass(), new BuildingPass(), new DecoPass()
        };

        public override bool ValidBiomeConditions(Player player)
        {
            return Area.Contains(player.Center.ToTileCoordinates());
        }

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
