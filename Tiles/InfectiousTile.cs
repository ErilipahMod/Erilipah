using Erilipah.Core;
using Erilipah.Tiles.Epicenter;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Erilipah.Tiles
{
    public abstract class InfectiousTile : ModTile
    {
        public static int Range => ConfigReader.Get<int>("infectious tile range");

        public static int Default => ModContent.TileType<InfectedGlob>();

        [AutoInit(InitHooks.Unload)]
        private static List<InfectiousTile> infectiousTiles;

        public abstract bool CanInfect(int type);

        public override bool Autoload(ref string name, ref string texture)
        {
            if (infectiousTiles == null)
            {
                infectiousTiles = new List<InfectiousTile>();
            }
            infectiousTiles.Add(this);
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            // Choose a random point to infect
            Point offset = (WorldGen.genRand.NextFloat().ToRotationVector2() * WorldGen.genRand.Next(Range)).ToPoint();
            Point tilePos = new Point(i + offset.X, j + offset.Y);

            TileExtensions.Infect(tilePos.X, tilePos.Y);
        }

        public static bool InfectTile(int i, int j)
        {
            // Don't infect any invalid tiles
            if (WorldGen.InWorld(i, j))
            {
                Tile tile = Framing.GetTileSafely(i, j);

                // Don't infect the jungle temple 'til Golem is dead
                if (tile.type == TileID.LihzahrdBrick && !NPC.downedGolemBoss)
                {
                    return false;
                }

                // If the tile is solid, then try to infect it
                if (WorldGen.SolidOrSlopedTile(i, j))
                {
                    // Check that the tile isn't already infected
                    if (!(TileLoader.GetTile(tile.type) is InfectiousTile))
                    {
                        // Find an infectious tile that can convert this tile type
                        InfectiousTile convertTo = infectiousTiles.FirstOrDefault(t => t.CanInfect(tile.type));
                        if (convertTo != null)
                        {
                            // If we found one, convert it
                            tile.type = convertTo.Type;
                        }
                        else
                        {
                            // If not, default to Infected Glob (Stone for now)
                            tile.type = (ushort)Default;
                        }
                        WorldGen.TileFrame(i, j);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}