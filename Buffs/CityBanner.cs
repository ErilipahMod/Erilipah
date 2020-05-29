using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Buffs
{
    public class CityBanner : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("City Banner");
            Description.SetDefault("The City's warmth slows your infection");
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.I().reductionRate *= 0.50f;
        }
    }
}