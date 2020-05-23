using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TFilters = Terraria.Graphics.Effects.Filters;

namespace Erilipah.Effects
{
    public static class ShaderLoader
    {
        public const string CoronaFx = "Erilipah:Corona";
        public const string ErilipahFx = "Erilipah:Biome";

        private static MiscShaderData GetMiscShader(string name, string pass) => new MiscShaderData(new Ref<Effect>(Erilipah.Instance.GetEffect(name)), pass);

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            if (!Main.dedServ)
            {
                //GameShaders.Misc[CoronaFx] = GetMiscShader("Effects/Distort", "CoronaFx");
                TFilters.Scene[ErilipahFx] = new Graphics.ErilipahFilter("Effects/ErilipahDistort", "Distort");
            }
        }
    }
}
