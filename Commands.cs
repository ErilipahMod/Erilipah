using Erilipah.Core;
using Erilipah.KeyItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Erilipah
{
    public class DevCommand : ModCommand
    {
        public override string Command => "dev";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            string type = args[0];
            switch (type)
            {
                case "unlock-lost-key":
                    KeyItemManager.Get<LostKey>().Unlock();
                    break;

                case "lock-lost-key":
                    LostKey obj = KeyItemManager.Get<LostKey>();
                    typeof(KeyItem).Property("Unlocked").SetValue(obj, false);
                    break;

                default:
                    caller.Reply("Not a thing");
                    break;
            }
        }
    }
}
