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
    public class CrystallineDust : ModDust
    {
        public override void SetDefaults()
        {
            Terraria.GameContent.ChildSafety.SafeDust[Type] = true;
        }

        public override void OnSpawn(Dust dust)
        {
            base.OnSpawn(dust);
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.velocity *= 0.9f;
            return true;
        }

        public override bool Update(Dust dust)
        {
            if (!dust.noLight)
            {
                Lighting.AddLight(dust.position, 0.1f, 0, 0.1f);
            }
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.03f;
                dust.rotation += dust.velocity.Y / 10;
            }
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
