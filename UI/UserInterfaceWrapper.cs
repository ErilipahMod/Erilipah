using System.Collections.Generic;
using Terraria.UI;

namespace Erilipah.UI
{
    public delegate void ModifyInterfaceDelegate(List<GameInterfaceLayer> layers);

    public class UserInterfaceWrapper
    {
        public UserInterfaceWrapper(UserInterface @interface, ModifyInterfaceDelegate modifyInterface)
        {
            Interface = @interface;
            ModifyInterface = modifyInterface;
        }

        public UserInterface Interface { get; set; }
        public ModifyInterfaceDelegate ModifyInterface { get; set; }
    }
}