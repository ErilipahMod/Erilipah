using Erilipah.KeyItems;
using Erilipah.Worldgen;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Erilipah.Core
{
    public class ErilipahPlayer : ModPlayer
    {
        public override void UpdateBiomes()
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.UpdateBiome(player);
            }
        }

        public override void UpdateBiomeVisuals()
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.OnUpdateVisuals();
            }
        }

        public override bool CustomBiomesMatch(Player other)
        {
            return BiomeManager.GetAll().All(b => b.GetInBiome(player) == b.GetInBiome(other));
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.SetInBiome(other, biome.GetInBiome(player));
            }
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                writer.Write(biome.GetInBiome(player));
            }
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            foreach (var biome in BiomeManager.GetAll())
            {
                biome.SetInBiome(player, reader.ReadBoolean());
            }
        }

        public override TagCompound Save()
        {
            TagCompound compound = new TagCompound();
            KeyItemManager.Save(compound);
            return compound;
        }

        public override void Load(TagCompound tag)
        {
            KeyItemManager.Load(tag);
        }
    }
}