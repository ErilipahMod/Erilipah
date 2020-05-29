using Erilipah.Core;
using Erilipah.Effects;
using Erilipah.Worldgen;
using Erilipah.Worldgen.Epicenter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Erilipah.Graphics
{
    public class ErilipahFilter : Filter
    {
        public ErilipahFilter(string file, string pass) : base(GetScreenShaderData(file, pass), EffectPriority.VeryHigh)
        {
        }

        public static ErilipahFilter Instance => Filters.Scene[ShaderLoader.ErilipahFx] as ErilipahFilter;

        public float Fade { get; private set; }

        private static ScreenShaderData GetScreenShaderData(string file, string pass)
        {
            return new ErilipahScreenShaderData(new Ref<Effect>(Erilipah.Instance.GetEffect(file)), pass);
        }

        private static float GetIntensityAmount()
        {
            return 1;
        }

        public override bool IsVisible()
        {
            return BiomeManager.Get<Epicenter>().TileCounts > 0 || Fade > 0;
        }

        private class ErilipahScreenShaderData : ScreenShaderData
        {
            private readonly Vector3 tintColor = new Vector3(0.1f, 0, 0.2f);

            public ErilipahScreenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
            {
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                float targetFade = Math.Min(1, BiomeManager.Get<Epicenter>().TileCounts / (float)BiomeManager.Get<Epicenter>().TileCountThreshold / 5);
                Instance.Fade += (targetFade - Instance.Fade) / 60;

                if (Instance.Fade < 0.01f && targetFade == 0)
                    Instance.Fade = 0;
            }

            public override void Apply()
            {
                UseIntensity(MathHelper.Lerp(
                    ConfigReader.Get<float>("visuals.erilipah.obstruction (low end)"),
                    ConfigReader.Get<float>("visuals.erilipah.obstruction (high end)"),
                    GetIntensityAmount())
                    );
                UseTargetPosition(Main.LocalPlayer.Center);
                UseColor(tintColor);
                UseOpacity(Instance.Fade);
                Shader.Parameters["uDesaturation"].SetValue(ConfigReader.Get<float>("visuals.erilipah.desaturation"));
                Shader.Parameters["uTint"].SetValue(ConfigReader.Get<float>("visuals.erilipah.tint"));

                base.Apply();
            }
        }
    }
}