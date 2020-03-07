using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Erilipah.UI.KeyItems
{
    public class KeyItemUIState : UIState
    {
        [field: AutoInit(InitHooks.Load | InitHooks.Unload)]
        public static KeyItemUIState Instance { get; }

        public KeyItemInventory Inventory { get; private set; }

        [HookLoading(LoadHooks.PostLoad)]
        private static void OnLoad()
        {
            UserInterface ui = new UserInterface();
            ui.SetState(Instance);
            Erilipah.Instance.UIs.Add(new UserInterfaceWrapper(
                ui,
                ui.GetModifyInterfaceDel("Vanilla: Inventory", "Erilipah: Key Items")
                ));
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            Inventory = new KeyItemInventory
            {
                Left = { Pixels = 66 },
                Top = { Pixels = Main.instance.invBottom },
                Width = { Pixels = 120 },
                Height = { Pixels = 40 }
            };
            Inventory.AddSlots(Main.inventoryBack15Texture, 200 / Main.inventoryBack15Texture.Width + 1, 6, 2);
            Append(Inventory);

            Texture2D buttonTexture = ModContent.GetTexture("Erilipah/UI/KeyItems/OpenKeyItemButton");
            OpenKeyItemButton button = new OpenKeyItemButton(buttonTexture)
            {
                Left = { Pixels = 422 },
                Top = { Pixels = Main.instance.invBottom },
                Width = { Pixels = buttonTexture.Width },
                Height = { Pixels = buttonTexture.Height }
            };
            button.OnClick += delegate { Inventory.Open ^= true; };
            Append(button);
        }
    }
}
