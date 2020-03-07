using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using Terraria;

namespace Erilipah.Effects.Handlers
{
    public class DistortHandler : ShaderHandler
    {
        private readonly Texture2D tex;
        private readonly Texture2D backingTex;
        private readonly Rectangle? texFrame;

        public DistortHandler(Texture2D tex, Texture2D backingTex, Rectangle? texFrame)
        {
            this.tex = tex;
            this.backingTex = backingTex;
            this.texFrame = texFrame;
        }

        protected override ShaderInfo Shader => new ShaderInfo("Effects/Distort", "Default", "Invert");

        protected override void Handle()
        {
            Main.graphics.GraphicsDevice.Textures[0] = backingTex;
            Shader.Parameters["TexSize"].SetValue(tex.Size());
            Shader.Parameters["BackingTexSize"].SetValue(backingTex.Size());
            Shader.Parameters["Frame"].SetValue(texFrame == null ? FromRect(tex.Bounds) : FromRect(texFrame.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector4 FromRect(Rectangle rect) => new Vector4(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
