using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Erilipah.Effects.Handlers
{
    public abstract class ShaderHandler
    {
        protected abstract ShaderInfo Shader { get; }

        public void Apply()
        {
            Handle();
            Shader.Apply();
        }

        protected abstract void Handle();

        protected class ShaderInfo
        {
            private readonly Ref<Effect> effect;
            private readonly EffectPass pass;

            public ShaderInfo(string shader, string technique, string passName)
            {
                effect = new Ref<Effect>(Erilipah.Instance.GetEffect(shader));
                pass = effect.Value.Techniques[technique].Passes[passName];
            }

            public void Apply() => pass.Apply();

            public EffectParameterCollection Parameters => effect.Value.Parameters;
        }
    }
}