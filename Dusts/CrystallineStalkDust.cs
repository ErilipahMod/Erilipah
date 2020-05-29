using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Erilipah.Dusts
{
    public class CrystallineStalkDust : ModDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Erilipah/Dusts/CrystallineDust";
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Terraria.GameContent.ChildSafety.SafeDust[Type] = true;
        }

        public override void OnSpawn(Dust dust)
        {
            base.OnSpawn(dust);
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.velocity *= 0.9f;
            return true;
        }

        public override bool Update(Dust dust)
        {
            dust.rotation += 0.08f;
            dust.velocity.Y -= 0.025f;
            dust.velocity.X = (float)Math.Sin(MathHelper.TwoPi * dust.scale) + Main.windSpeed * 2;
            dust.scale -= 0.01f;
            if (dust.scale <= 0)
            {
                dust.active = false;
            }
            dust.position += dust.velocity;
            return false;
        }
    }
}
