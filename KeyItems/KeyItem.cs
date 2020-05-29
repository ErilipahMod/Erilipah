using Erilipah.UI.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Erilipah.KeyItems
{
    public abstract partial class KeyItem
    {
        private bool unlocking = false;

        public bool Unlocked { get; private set; } = false;

        public KeyItemSlot Container { get; set; }

        public virtual string Texture => GetType().FullName.Replace('.', '/');

        public virtual Rectangle? Frame => null;

        public void Unlock(Vector2 origin)
        {
            if (Unlocked || unlocking)
            {
                return;
            }

            if (!Main.playerInventory)
            {
                Main.LocalPlayer.ToggleInv();
            }

            UnlockAnimation anim = new UnlockAnimation(this, origin - Main.screenPosition);
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
    }
}