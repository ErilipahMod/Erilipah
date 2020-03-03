using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Erilipah.UI.KeyItems
{
    public class KeyItemUIState : UIState
    {
        [HookLoading(LoadHooks.PostLoad)]
        private static void OnLoad()
        {
            UserInterface ui = new UserInterface();
            ui.SetState(new KeyItemUIState());

            Erilipah.Instance.UIs.Add(new UserInterfaceWrapper(
                ui,
                ui.GetModifyInterfaceDel("Vanilla: Inventory", "Erilipah: Key Items")
                ));
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            KeyItemInventory inventory = new KeyItemInventory
            {
                Left = { Pixels = 73 },
                Top = { Pixels = Main.instance.invBottom },
                Width = { Pixels = 120 },
                Height = { Pixels = 40 }
            };
            inventory.AddSlots(Main.inventoryBack15Texture, 6, 3); // TODO make a config for this shit
            Append(inventory);

            Texture2D buttonTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            OpenKeyItemButton button = new OpenKeyItemButton(buttonTexture)
            {
                Left = { Pixels = 410 },
                Top = { Pixels = 270 },
                Width = { Pixels = buttonTexture.Width },
                Height = { Pixels = buttonTexture.Height }
            };
            button.OnClick += delegate { inventory.Open ^= true; };
            Append(button);
        }
    }
}
