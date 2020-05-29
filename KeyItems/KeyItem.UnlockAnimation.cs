using Erilipah.Runnables;
using Erilipah.UI.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.KeyItems
{
    public abstract partial class KeyItem
    {
        // Used just for the unlock animation lol
        private class UnlockAnimation : UIAnimation
        {
            private const float lerpMin = -2.5f;

            private readonly ParticleCollection particles = new ParticleCollection();
            private readonly KeyItem item;

            private Vector2 at;
            private float lerp = lerpMin;
            private bool boom = false;

            public override string AboveLayer => "Vanilla: Info Accessories Bar";

            public UnlockAnimation(KeyItem item, Vector2 origin)
            {
                this.item = item;
                On.Terraria.Player.ToggleInv += PreventInventoryToggle;
                OnFinish += () =>
                {
                    On.Terraria.Player.ToggleInv -= PreventInventoryToggle;
                };
                at = origin;
            }

            private static void PreventInventoryToggle(On.Terraria.Player.orig_ToggleInv orig, Player self)
            {
            }

            public override void Update(GameTime gameTime)
            {
                particles.UpdateAll();

                Lighting.AddLight(GetPosition(), 1f, 1f, 1f);

                Main.playerInventory = true;
                KeyItemUIState.Instance.Inventory.Open = true;

                lerp += 1 / 90f;

                // Jump up from origin
                if (lerp < -1f)
                {
                    at.Y += MathHelper.Lerp(-3.5f, 0, 1 - (lerp + 1) / (lerpMin + 1));
                }
                // Suck in particles
                else if (lerp < -0.15f)
                {
                    Vector2 rand = Main.rand.NextVector2CircularEdge(120, 120);
                    Vector2 pos = GetPosition() + rand;
                    Vector2 acc = -rand.SafeNormalize(Vector2.Zero) / 4;
                    particles.Add(new GlowyParticle()
                    {
                        Position = pos,
                        Acceleration = acc,
                        Velocity = acc * 10,
                        RotationChange = Main.rand.NextBool().ToDirectionInt() * 0.15f,
                        ScaleChange = -0.04f
                    });
                }
                // Explode
                else if (lerp > 0f && !boom)
                {
                    boom = true;
                    for (int i = 0; i < 35; i++)
                    {
                        Vector2 pos = GetPosition();
                        Vector2 acc = Main.rand.NextVector2CircularEdge(1, 1);
                        particles.Add(new GlowyParticle()
                        {
                            Position = pos,
                            Acceleration = acc,
                            Velocity = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(10, 20),
                            RotationChange = Main.rand.NextBool().ToDirectionInt() * 0.15f,
                            ScaleChange = -0.03f
                        });
                    }
                }
                // Last bit, die
                else if (lerp > 1f)
                {
                    Finish();
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                particles.DrawAll(spriteBatch);

                Texture2D texture2D = ModContent.GetTexture(item.Texture);
                Vector2 origin = item.Frame?.Center.ToVector2() ?? texture2D.Size() / 2;
                spriteBatch.Draw(texture2D, GetPosition(), item.Frame, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }

            private Vector2 GetPosition()
            {
                // Until lerp is > 0, just draw in the middle of the screen
                if (lerp > 0)
                {
                    Vector2 end = item.Container.GetDimensions().Center();
                    return Vector2.SmoothStep(at, end, lerp);
                }
                return at;
            }

            private class GlowyParticle : Particle
            {
                public GlowyParticle() : base(ModContent.GetTexture("Erilipah/KeyItems/WhiteParticle"))
                {
                }

                public override void Update()
                {
                    base.Update();

                    Lighting.AddLight(Position + Main.screenPosition, Scale, Scale, Scale);
                }
            }
        }
    }
}