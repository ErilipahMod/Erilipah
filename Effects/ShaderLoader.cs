//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.Graphics.Effects;
//using Terraria.Graphics.Shaders;
//using Terraria.ModLoader;

//namespace Erilipah.Effects
//{
//    public static class ShaderLoader
//    {
//        public const string CoronaFx = "Erilipah:Corona";

//        private static MiscShaderData Get(string name, string pass) => new MiscShaderData(new Ref<Effect>(Erilipah.Instance.GetEffect(name)), pass);

//        [HookLoading(LoadHooks.Load)]
//        private static void OnLoad()
//        {
//            if (!Main.dedServ)
//            {
//                GameShaders.Misc[CoronaFx] = Get("Effects/Corona", "CoronaFx");
//            }
//        }
//    }
//}
