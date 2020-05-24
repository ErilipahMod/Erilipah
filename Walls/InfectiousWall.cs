using Erilipah.Tiles;
using Erilipah.Walls.Epicenter;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Walls
{
    public abstract class InfectiousWall : ModWall
    {
        public static int Default => ModContent.WallType<InfectedStoneWall>();

        [AutoInit(InitHooks.Unload)]
        private static List<InfectiousWall> infectiousWalls;

        public abstract bool CanInfect(int type);

        public override bool Autoload(ref string name, ref string texture)
        {
            if (infectiousWalls == null)
            {
                infectiousWalls = new List<InfectiousWall>();
            }
            infectiousWalls.Add(this);
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            // Choose a random point to infect
            Point offset = (WorldGen.genRand.NextFloat().ToRotationVector2() * WorldGen.genRand.Next(InfectiousTile.Range)).ToPoint();
            Point tilePos = new Point(i + offset.X, j + offset.Y);

            TileExtensions.Infect(tilePos.X, tilePos.Y);
        }

        public static bool InfectWall(int i, int j)
        {
            // Don't infect invalid walls
            if (WorldGen.InWorld(i, j))
            {
                Tile tile = Framing.GetTileSafely(i, j);

                // No Lihzahrd infection pre-golem
                if (tile.wall == WallID.LihzahrdBrickUnsafe && !NPC.downedGolemBoss)
                {
                    return false;
                }

                // Check if we should bother
                if (tile.wall != WallID.None)
                {
                    // Check that the tile isn't already infected
                    if (!(WallLoader.GetWall(tile.wall) is InfectiousWall))
                    {
                        // Find a matching wall
                        InfectiousWall convertTo = infectiousWalls.FirstOrDefault(t => t.CanInfect(tile.wall));
                        if (convertTo != null)
                        {
                            // If we found one, convert it
                            tile.wall = convertTo.Type;
                            return true;
                        }
                        else
                        {
                            // If not, default to Infected Glob (Stone for now)
                            tile.wall = (ushort)Default;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
