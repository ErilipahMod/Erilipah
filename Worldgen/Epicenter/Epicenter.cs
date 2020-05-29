using Erilipah.Core;
using Erilipah.Effects;
using Erilipah.Graphics;
using Erilipah.Tiles.Epicenter;
using Erilipah.Tiles.LostCity;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using TFilters = Terraria.Graphics.Effects.Filters;

namespace Erilipah.Worldgen.Epicenter
{
    public partial class Epicenter : Biome
    {
        public static int SurfaceWidth { get; } = ConfigReader.Get<int>("worldgen.epicenter.surface width");

        public Rectangle Area { get; private set; }

        public override int TileCountThreshold => 500;

        public override IEnumerable<int> BiomeTileTypes { get; } = new[]
        {
            TileType<InfectedStone>(), TileType<InfectedSoil>(), TileType<InfectedGlob>(), TileType<LostBrick>(), TileType<LostBrickUnsafe>()
        };

        public override IEnumerable<IBiomeGenPass> BiomeGenPasses => new IBiomeGenPass[]
        {
            new PlantPass(), new AreaPass()
        };

        public override void OnUpdateVisuals()
        {
            if (ErilipahFilter.Instance.IsVisible())
            {
                TFilters.Scene.Activate(ShaderLoader.ErilipahFx, Main.LocalPlayer.Center);
            }
            else
            {
                TFilters.Scene[ShaderLoader.ErilipahFx].Deactivate();
            }
        }

        public override void ModifyMusic(ref int music, ref MusicPriority priority)
        {
            priority = MusicPriority.BiomeHigh;
            music = Erilipah.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/Erilipah");
        }

        public override void ModifySunlight(ref Color tileColor, ref Color backgroundColor)
        {
            float fade = ErilipahFilter.Instance.Fade;
            backgroundColor = Color.Lerp(backgroundColor, Color.Black, 0.65f * fade);
            tileColor = Color.Lerp(tileColor, Color.Black, 0.5f * fade);
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