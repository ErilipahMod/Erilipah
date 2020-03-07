using Erilipah.Runnables;
using Erilipah.UI.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Erilipah.KeyItems
{
    public abstract partial class KeyItem
    {
        // Used just for the unlock animation lol
        private class UnlockAnimation : UIAnimation
        {
            private readonly ParticleCollection particles = new ParticleCollection();

            private readonly KeyItem item;

            private float lerp = -1.5f;

            private bool boom = false;

            public override string AboveLayer => "Vanilla: Info Accessories Bar";

            public UnlockAnimation(KeyItem item)
            {
                this.item = item;
                On.Terraria.Player.ToggleInv += PreventInventoryToggle;
                Filters.Scene.OnPostDraw += DrawParticles;
                OnFinish += () =>
                {
                    On.Terraria.Player.ToggleInv -= PreventInventoryToggle;
                    Filters.Scene.OnPostDraw -= DrawParticles;
                };
            }

            private static void PreventInventoryToggle(On.Terraria.Player.orig_ToggleInv orig, Player self) { }

            public override void Update(GameTime gameTime)
            {
                particles.UpdateAll();

                Main.playerInventory = true;
                KeyItemUIState.Instance.Inventory.Open = true;

                lerp += 1 / 80f;

                // First bit, just generate particles
                if (lerp < -0.25)
                {
                    particles.Add(new InvertParticle(GetDrawPosition(), Main.rand.NextVector2CircularEdge(100, 100)));
                }
                else if (lerp > 0 && !boom)
                {
                    boom = true;
                    for (int i = 0; i < 35; i++)
                    {
                        InvertParticle particle = new InvertParticle(GetDrawPosition(), Main.rand.NextVector2CircularEdge(5, 5));
                        particle.Acceleration *= -1.5f + Main.rand.NextFloat(-1f, -0.2f);
                        particle.Velocity *= -1.5f + Main.rand.NextFloat(-3.5f, 0);
                        particles.Add(particle);
                    }
                }
                // Last bit, die
                else if (lerp > 1)
                {
                    Finish();
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Texture2D texture2D = ModContent.GetTexture(item.Texture);
                Vector2 origin = item.Frame?.Center.ToVector2() ?? texture2D.Size() / 2;
                spriteBatch.Draw(texture2D, GetDrawPosition(), item.Frame, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }

            private void DrawParticles()
            {
                particles.DrawAll(Main.spriteBatch);
            }

            private Vector2 GetDrawPosition()
            {
                // Until lerp is > 0, just draw in the middle of the screen
                Vector2 start = new Vector2(Main.screenWidth, Main.screenHeight - 125 * 2) / 2;
                if (lerp > 0)
                {
                    Vector2 end = item.Container.GetDimensions().Center();
                    return Vector2.SmoothStep(start, end, lerp);
                }
                return start;
            }
        }
    }
}
