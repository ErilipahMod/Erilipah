using Erilipah.Dusts;
using Microsoft.Xna.Framework;
using NetEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Packets.Effects
{
    [Serializable]
    public class SpawnCrystallineStalkDust : Module
    {
        private readonly Vector2 position;

        public SpawnCrystallineStalkDust(Vector2 position)
        {
            this.position = position;
        }

        protected override void Receive()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<CrystallineStalkDust>(), Scale: 1.6f);
            }
        }
    }
}
