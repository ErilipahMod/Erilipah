using Erilipah.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace Erilipah.UI.KeyItems
{
    public class KeyItemSlot : UIElement
    {
        public KeyItemSlot(Texture2D backgroundTexture)
        {
            BackgroundTexture = backgroundTexture;
        }

        public KeyItem Contained { get; set; }

        public Texture2D BackgroundTexture { get; set; }

        public bool Visible { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!ReferenceEquals(Contained?.Container, this))
            {
                Contained = null;
            }

            Contained?.Update();
        }

        public override void OnActivate()
        {
            base.OnActivate();

            if (Contained != null)
            {
                Contained.Container = this;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(BackgroundTexture, GetDimensions().ToRectangle(), null, Color.White);
                Contained?.Draw(spriteBatch);
            }
        }
    }
}
