using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Erilipah.UI.KeyItems
{
    public class OpenKeyItemButton : UIImageButton
    {
        public OpenKeyItemButton(Texture2D texture) : base(texture) { }

        public override void Update(GameTime gameTime)
        {
            MarginLeft = 0;
            MarginTop = 0;
            if ((Main.player[Main.myPlayer].chest != -1 || Main.npcShop > 0) && !Main.recBigList)
            {
                MarginLeft += 5;
                MarginTop += 168;
            }
            else if ((Main.player[Main.myPlayer].chest == -1 || Main.npcShop == -1) && Main.trashSlotOffset != Terraria.DataStructures.Point16.Zero)
            {
                MarginLeft += Main.trashSlotOffset.X;
                MarginTop += Main.trashSlotOffset.Y;
            }

            if (IsMouseHovering)
            {
                Main.blockMouse = true;
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            if (Main.playerInventory)
            {
                base.MouseOver(evt);
            }
        }

        public override void Click(UIMouseEvent evt)
        {
            if (Main.playerInventory)
            {
                base.Click(evt);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory)
            {
                base.DrawSelf(spriteBatch);
            }
        }
    }
}
