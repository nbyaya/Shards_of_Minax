using System;
using Server.Items;

namespace Server.Mobiles
{
    public class ParoxysmusSwampDragon : SwampDragon
    {
        [Constructable]
        public ParoxysmusSwampDragon()
            : base()
        {
            Name = "Chief Paroxysmus' Swamp Dragon";

            BardingResource = CraftResource.Iron;
            BardingExceptional = true;
            BardingHP = BardingMaxHP;
            HasBarding = true;
            Hue = 1155;
        }

        public ParoxysmusSwampDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteOnRelease { get { return true; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1049646); // (summoned)
        }

        public override void GenerateLoot()
        {
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new ParoxysmusBreastplate());
            }
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
