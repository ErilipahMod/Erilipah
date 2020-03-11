using Erilipah.Core;
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

namespace Erilipah.Filters
{
    public class ErilipahFilter : Filter
    {
        private static float Intensity => MathHelper.Lerp(
            ConfigReader.Get<float>("visuals.erilipah.obstruction (low end)"),
            ConfigReader.Get<float>("visuals.erilipah.obstruction (high end)"), 
            (float)GetIntensityAmount());
        private static float Desaturation { get; } = ConfigReader.Get<float>("visuals.erilipah.desaturation");
        private static float Tint { get; } = ConfigReader.Get<float>("visuals.erilipah.tint");
        private static Vector3 TintColor { get; } = new Vector3(0.2f, 0, 0.4f);

        public ErilipahFilter(string file, string pass) : base(GetScreenShaderData(file, pass), EffectPriority.VeryHigh) { }

        private static ScreenShaderData GetScreenShaderData(string file, string pass)
        {
            return new ErilipahScreenShaderData(new Ref<Effect>(Erilipah.Instance.GetEffect(file)), pass);
        }

        private static double GetIntensityAmount()
        {
            // If we're on the surface
            if (Main.LocalPlayer.Center.X < Main.rockLayer * 16)
            {
                // Cycle through night & day
                var dayCycle = Main.dayTime ? Main.dayLength : Main.nightLength;  
                var cycle = Math.Sin(Math.Abs(Main.time - dayCycle / 2) / (dayCycle / 2));
                return -(cycle - 1) / 2;
            }
            // Darkness the deeper you are.
            return Main.LocalPlayer.Center.Y / 16 / Main.maxTilesY;
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
                UseIntensity(ErilipahFilter.Intensity);
                UseTargetPosition(Main.LocalPlayer.Center);
                UseColor(TintColor);
                Shader.Parameters["uDesaturation"].SetValue(Desaturation);
                Shader.Parameters["uTint"].SetValue(Tint);

                base.Apply();
            }
        }
    }
}
