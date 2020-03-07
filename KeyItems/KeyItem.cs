using Erilipah.Runnables;
using Erilipah.UI.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Erilipah.KeyItems
{
    public abstract class KeyItem
    {
        private bool unlocking = false;

        public bool Unlocked { get; private set; } = false;

        public KeyItemSlot Container { get; set; }

        public virtual string Texture => GetType().FullName.Replace('.', '/');

        public virtual Rectangle? Frame => null;

        public void Unlock()
        {
            if (Unlocked || unlocking)
            {
                return;
            }

            if (!Main.playerInventory)
            {
                Main.LocalPlayer.ToggleInv();
            }

            UnlockAnimation anim = new UnlockAnimation(this);
            anim.OnFinish += () =>
            {
                Unlocked = true;
                unlocking = false;
            };
            anim.Start();
            unlocking = true;
        }

        /// <summary>
        /// Updates the KeyItem every tick. Clientside.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Draws the KeyItem. Use the <see cref="Container"/> property's <see cref="Terraria.UI.UIElement.GetDimensions()"/> method to get the draw position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var dimensions = Container.GetDimensions();

            var texture2D = ModContent.GetTexture(Texture);
            var color = Unlocked ? Color.White : Color.Black;
            var origin = Frame?.Center.ToVector2() ?? texture2D.Size() / 2;
            var scale = dimensions.ToRectangle().Area() / (float)texture2D.Bounds.Area();

            spriteBatch.Draw(texture2D, dimensions.Center(), Frame, color, 0, origin, Math.Min(1, scale), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Saves custom data. Base: Saves <see cref="Unlocked"/>.
        /// </summary>
        /// <param name="compound">The <see cref="TagCompound"/> to save to.</param>
        public virtual void Save(TagCompound compound)
        {
            compound.Add(nameof(Unlocked), Unlocked);
        }

        /// <summary>
        /// Loads custom data. Base: Loads <see cref="Unlocked"/>.
        /// </summary>
        /// <param name="compound">The <see cref="TagCompound"/> to load from.</param>
        public virtual void Load(TagCompound compound)
        {
            Unlocked = compound.Get<bool>(nameof(Unlocked));
        }

        /// <summary>
        /// Called when the KeyItem is first instantiated as a singleton on load.
        /// </summary>
        public virtual void OnInitialize() { }

        // Used just for the unlock animation lol
        private class UnlockAnimation : UIAnimation
        {
            private readonly KeyItem item;

            private float lerp = 0;

            public override string AboveLayer => "Vanilla: Info Accessories Bar";

            public UnlockAnimation(KeyItem item)
            {
                this.item = item;
                On.Terraria.Player.ToggleInv += Player_ToggleInv;
                OnFinish += () => On.Terraria.Player.ToggleInv -= Player_ToggleInv;
            }

            private static void Player_ToggleInv(On.Terraria.Player.orig_ToggleInv orig, Player self) { }

            public override void Update(GameTime gameTime)
            {
                LockInventory();

                lerp += 1 / 100f;
                if (lerp > 1)
                {
                    Finish();
                }
            }

            private static void LockInventory()
            {
                Main.playerInventory = true;
                KeyItemUIState.Instance.Inventory.Open = true;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Texture2D texture2D = ModContent.GetTexture(item.Texture);
                Vector2 origin = item.Frame?.Center.ToVector2() ?? texture2D.Size() / 2;
                spriteBatch.Draw(texture2D, GetDrawPosition(), item.Frame, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }

            private Vector2 GetDrawPosition()
            {
                Vector2 start = new Vector2(Terraria.Main.screenWidth, Terraria.Main.screenHeight) / 2;
                Vector2 end = item.Container.GetDimensions().Center();
                return Vector2.SmoothStep(start, end, lerp);
            }
        }
    }
}
