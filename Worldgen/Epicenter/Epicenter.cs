using Erilipah.Core;
using Erilipah.Tiles.Epicenter;
using Erilipah.Tiles.LostCity;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;
using TFilters = Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;
using Erilipah.Effects;

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

        public override void OnUpdateVisuals()
        {
            if (GetInBiome(Main.LocalPlayer))
            {
                TFilters.Scene.Activate(ShaderLoader.ErilipahFx, Main.LocalPlayer.Center);
            }
            else
            {
                TFilters.Scene[ShaderLoader.ErilipahFx].Deactivate();
            }
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
