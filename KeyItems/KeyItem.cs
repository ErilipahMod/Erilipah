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
        public bool Unlocked { get; private set; } = false;

        public virtual string Texture => GetType().FullName.Replace('.', '/');

        public virtual Rectangle? Frame => null;

        public void Unlock() => Unlocked = true;

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle dimensions)
        {
            Texture2D texture2D = ModContent.GetTexture(Texture);
            Color color = Unlocked ? Color.White : Color.Black;
            Vector2 origin = texture2D.Size() / 2;
            float scale = dimensions.Area() / (float)texture2D.Bounds.Area();

            spriteBatch.Draw(texture2D, dimensions.Center.ToVector2(), Frame, color, 0, origin, Math.Min(1, scale), SpriteEffects.None, 0);
        }

        public virtual void Hover() { }

        public virtual void Save(TagCompound compound)
        {
            compound.Add(nameof(Unlocked), Unlocked);
        }

        public virtual void Load(TagCompound compound)
        {
            Unlocked = compound.Get<bool>(nameof(Unlocked));
        }

        public virtual void OnInitialize() { }
    }
}
