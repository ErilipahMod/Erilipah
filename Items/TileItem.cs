using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Items
{
    public abstract class TileItem<T> : ModItem where T : ModTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            item.CloneDefaults(ItemID.DirtBlock);
            item.createTile = ModContent.TileType<T>();
        }
    }
}