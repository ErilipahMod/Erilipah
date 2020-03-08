using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Erilipah.Effects
{
    public class CoronaHandler : ShaderHandler
    {
        private readonly Texture2D tex;
        private readonly Rectangle? frame;
        private readonly Color color;

        public CoronaHandler(Texture2D tex, Rectangle? frame, Color color, string pass) : base("Effects/Corona", "Technique1", pass)
        {
            this.tex = tex;
            this.frame = frame;
            this.color = color;
        }

        protected override void Handle(Effect effect)
        {
            Main.graphics.GraphicsDevice.Textures[0] = tex;
            effect.Parameters["uColor"].SetValue(color.ToVector3());
            effect.Parameters["uTime"].SetValue(Main.GlobalTime);
            effect.Parameters["uSourceRect"].SetValue(FromRect(frame ?? tex.Bounds));
            effect.Parameters["uImageSize0"].SetValue(tex.Size());
        }

        private static Vector4 FromRect(Rectangle rect) => new Vector4(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
