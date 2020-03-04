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

        public IEnumerable<KeyItemSlot> Slots => Elements.OfType<KeyItemSlot>();

        public bool Open
        {
            get => open;
            set
            {
                if (open != value)
                {
                    if (open = value)
                    {
                        Main.npcChatText = string.Empty;
                        Main.editChest = false;
                        Main.editSign = false;
                        Main.npcShop = 0;
                        Main.LocalPlayer.talkNPC = -1;
                        Main.LocalPlayer.sign = -1;
                        Main.LocalPlayer.chest = -1;
                    }
                    foreach (var slot in Slots)
                    {
                        slot.Visible = open;
                    }
                }
            }
        }

        public void AddSlots(Texture2D backgroundTexture, int slotsPerRow, int rowSpacing, int columnSpacinng)
        {
            int iteration = 0;
            foreach (var item in KeyItemManager.GetAll())
            {
                MinWidth.Pixels += backgroundTexture.Width;
                MinHeight.Pixels += backgroundTexture.Height;
                KeyItemSlot slot = new KeyItemSlot(backgroundTexture)
                {
                    Left = { Pixels = iteration % slotsPerRow * (backgroundTexture.Width + columnSpacinng) },
                    Top = { Pixels = iteration / slotsPerRow * (backgroundTexture.Height + rowSpacing) },
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

            if ((!Main.playerInventory || !string.IsNullOrEmpty(Main.npcChatText) || Main.editChest || Main.LocalPlayer.talkNPC != -1 || Main.LocalPlayer.sign != -1 || Main.LocalPlayer.chest > -1) && Open)
            {
                Open = false;
            }
        }
    }
}
