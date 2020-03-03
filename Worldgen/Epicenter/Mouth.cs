using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Mouth : Biome
    {
        public Rectangle Basin { get; private set; }
        public Rectangle Tunnels { get; private set; }

        public override IEnumerable<int> BiomeTileTypes => new[]
        {
            TileType<Tiles.Epicenter.InfectedStone>()
        };

        public override IEnumerable<IBiomeGenPass> BiomeGenPasses => new IBiomeGenPass[]
        {
            new MouthPass(), new TunnelPass()
        };

        public override bool ValidBiomeConditions(Player player) => player.Center.Y < Main.worldSurface * 16;

        public override void Save(TagCompound compound)
        {
            compound[nameof(Basin)] = Basin;
            compound[nameof(Tunnels)] = Tunnels;
        }
        public override void Load(TagCompound compound)
        {
            Basin = compound.Get<Rectangle>(nameof(Basin));
            Tunnels = compound.Get<Rectangle>(nameof(Tunnels));
        }
    }
}
