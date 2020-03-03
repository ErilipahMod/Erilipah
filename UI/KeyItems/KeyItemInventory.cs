using Erilipah.KeyItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace Erilipah.UI.KeyItems
{
    public class KeyItemInventory : UIElement
    {
        private bool open;

        public bool Open
        {
            get => open;
            set
            {
                if (open == value)
                {
                    return;
                }

                if (open = value)
                {
                    Main.editChest = false;
                    Main.editSign = false;
                    Main.npcShop = -1;
                    Main.LocalPlayer.chest = -1;
                }

                foreach (var slot in Elements.OfType<KeyItemSlot>())
                {
                    slot.Visible = open;
                }
            }
        }

        public void AddSlots(Texture2D backgroundTexture, int rowSize, int columnSize)
        {
            int iteration = 0;
            foreach (var item in KeyItemManager.GetAll())
            {
                MinWidth.Pixels += backgroundTexture.Width;
                MinHeight.Pixels += backgroundTexture.Height;
                KeyItemSlot slot = new KeyItemSlot(backgroundTexture)
                {
                    HAlign = iteration % 5 / (float)rowSize,
                    VAlign = iteration / 5 / (float)columnSize,
                    Width = { Pixels = backgroundTexture.Width },
                    Height = { Pixels = backgroundTexture.Height },
                    Contained = item
                };
                Append(slot);
                iteration++;
            }
            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if ((!Main.playerInventory || Main.npcShop > -1 || Main.LocalPlayer.chest > -1) && Open)
            {
                Open = false;
            }
        }
    }
}
