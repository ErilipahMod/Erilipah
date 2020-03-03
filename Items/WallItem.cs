using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Items
{
    public abstract class WallItem<T> : ModItem where T : ModWall
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            item.CloneDefaults(ItemID.DirtWall);
            item.createWall = ModContent.WallType<T>();
        }
    }
}
