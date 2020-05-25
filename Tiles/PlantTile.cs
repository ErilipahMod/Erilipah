using Erilipah.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Tiles
{
    public abstract class PlantTile : ModTile
    {
        [AutoInit(InitHooks.Unload)]
        public static MutableLookup<int, PlantTile> Plants;

        public override bool Autoload(ref string name, ref string texture)
        {
            if (Plants == null)
            {
                Plants = new MutableLookup<int, PlantTile>();
            }
            foreach (var tileType in GrowthTiles)
            {
                Plants.Add(tileType, this);
            }
            return base.Autoload(ref name, ref texture);
        }

        public abstract IEnumerable<int> GrowthTiles { get; }
        public abstract bool ShouldGrow(int i, int j);
        public abstract void Grow(int i, int j);
    }
}
