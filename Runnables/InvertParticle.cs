using Erilipah.Effects.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Runnables
{
    public class InvertParticle : Particle
    {
        public InvertParticle() : base(ModContent.GetTexture("Erilipah/Runnables/InvertParticle")) { }

        public InvertParticle(Vector2 middle, Vector2 offset) : this()
        {
            Position = middle + offset;
            Acceleration = (middle - Position).SafeNormalize(Vector2.Zero) / 5f;
            RotationChange = Main.rand.NextBool().ToDirectionInt() * 0.15f;
            Velocity = Acceleration * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // FINISH figure out compliccaciicated drawing stuff lol
            // Start by testing w/o shader, then with
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            new DistortHandler(texture, Erilipah.Instance.ScreenPostFilters, null).Apply();
            base.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update()
        {
            base.Update();

            Scale -= 0.04f;
        }
    }
}
