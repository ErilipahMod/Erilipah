using Erilipah.Tiles.Epicenter.Plants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Projectiles
{
    public class Spore : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        // ai[0] = acceleration x
        // ai[1] = acceleration y

        private int syncTimer = 0;

        public override void AI()
        {
            // Add light and pulsing
            projectile.scale = (float)(Math.Sin(Main.GlobalTime) / 3 + 1.3);
            Lighting.AddLight(projectile.Center, new Vector3(0.3f, 0, 0.3f) * projectile.scale);

            var (i, j) = ((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16));
            var tile = Framing.GetTileSafely(i, j);
            if (tile.wall == 0)
            {
                // Accelerate according to the wind
                if (projectile.ai[0] < Main.windSpeed)
                {
                    projectile.ai[0] += Main.windSpeed / 60;
                }
            }

            var shroom = ModContent.GetInstance<ErilipahShroom>();

            if (WorldGen.SolidTile(tile) && shroom.GrowthTiles.Contains(tile.type) && shroom.GrowthConditions(i, j))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    shroom.Grow(i, j);
                }
                projectile.Kill();
                return;
            }

            // Random aerodynamic forces
            projectile.ai[0] += Main.rand.NextFloat(-0.01f, 0.01f);
            projectile.ai[1] += Main.rand.NextFloat(-0.01f, 0.01f);

            // Update velocity
            projectile.velocity.X += projectile.ai[0];
            projectile.velocity.Y += projectile.ai[1];

            // Cap velocity
            if (projectile.velocity.LengthSquared() > 1.5f * 1.5f)
            {
                projectile.velocity.Normalize();
                projectile.velocity *= 1.5f;
            }

            // Apply rotation
            projectile.rotation += projectile.velocity.X;

            // Apply friction
            projectile.velocity *= 0.98f;
            projectile.ai[0] *= 0.98f;
            projectile.ai[1] *= 0.98f;

            // Occasional sync
            if (Main.netMode == NetmodeID.Server && syncTimer++ % 120 == 0)
            {
                projectile.netUpdate = true;
            }
        }
    }
}
