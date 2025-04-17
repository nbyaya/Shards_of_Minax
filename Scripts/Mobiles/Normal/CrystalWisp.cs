using System;
using Server.Items;

namespace Server.Mobiles
{
    public class CrystalWisp : Wisp
    {
        [Constructable]
        public CrystalWisp()
        {
            this.Name = "a crystal wisp";
            this.Hue = 0x482;

            for (int i = 0; i < Utility.RandomMinMax(0, 1); i++)
            {
                this.PackItem(Loot.RandomScroll(0, Loot.ArcanistScrollTypes.Length, SpellbookType.Arcanist));
            }
        }

        public CrystalWisp(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.MedScrolls);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new LuminaCrystalRobe());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}