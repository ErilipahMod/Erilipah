using Erilipah.Core;
using Erilipah.Effects;
using Erilipah.Worldgen;
using Erilipah.Worldgen.Epicenter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Erilipah.Graphics
{
    public class ErilipahFilter : Filter
    {
        public static ErilipahFilter Instance => Filters.Scene[ShaderLoader.ErilipahFx] as ErilipahFilter;

        public float IntensityMultiplier { get; set; } = 1;
        public float IntensityRaw => MathHelper.Lerp(
            ConfigReader.Get<float>("visuals.erilipah.obstruction (low end)"),
            ConfigReader.Get<float>("visuals.erilipah.obstruction (high end)"),
            (float)GetIntensityAmount());
        public float Desaturation => ConfigReader.Get<float>("visuals.erilipah.desaturation");
        public float Tint => ConfigReader.Get<float>("visuals.erilipah.tint");
        public Vector3 TintColor { get; } = new Vector3(0.1f, 0, 0.2f);

        public ErilipahFilter(string file, string pass) : base(GetScreenShaderData(file, pass), EffectPriority.VeryHigh) { }

        private static ScreenShaderData GetScreenShaderData(string file, string pass)
        {
            return new ErilipahScreenShaderData(new Ref<Effect>(Erilipah.Instance.GetEffect(file)), pass);
        }

        private static double GetIntensityAmount()
        {
            // Cycle through night & day
            var dayCycle = Main.dayTime ? Main.dayLength : Main.nightLength;
            var cycle = Math.Sin(Math.Abs(Main.time - dayCycle / 2) / (dayCycle / 2));
            var normalizedCycle = (cycle + 1) / 2;
            var depth = Main.LocalPlayer.Center.Y / 16 / Main.maxTilesY;
            return normalizedCycle / 2 * (1f + depth);
        }

        public override bool IsVisible()
        {
            return BiomeManager.Get<Epicenter>().GetInBiome(Main.LocalPlayer) || Opacity > 0;
        }

        private class ErilipahScreenShaderData : ScreenShaderData
        {
            public ErilipahScreenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
            {
            }

            public override void Apply()
            {
                UseIntensity(Instance.IntensityRaw * Instance.IntensityMultiplier);
                UseTargetPosition(Main.LocalPlayer.Center);
                UseColor(Instance.TintColor);
                Shader.Parameters["uDesaturation"].SetValue(Instance.Desaturation);
                Shader.Parameters["uTint"].SetValue(Instance.Tint);

                base.Apply();
            }
        }
    }
}
