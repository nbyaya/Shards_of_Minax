using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class GrandmastersRobe : BaseClothingOfCommand
    {
        [Constructable]
        public GrandmastersRobe() : base(3, 0x1F03) // 0x1F03 is the item ID for a robe
        {
            Weight = 3.0;
            Name = "The Grandmaster's Robe";
            Hue = Utility.RandomMinMax(1150, 1175);
            Layer = Layer.OuterTorso;
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent != null && parent is PlayerMobile)
            {
                PlayerMobile pm = parent as PlayerMobile;
                pm.Skills[SkillName.AnimalTaming].Base += 20;
                pm.Skills[SkillName.Magery].Base += 20;
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent != null && parent is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)parent;
                pm.Skills[SkillName.AnimalTaming].Base -= 20;
                pm.Skills[SkillName.Magery].Base -= 20;
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Animal Taming +20");
            list.Add("Magery +20");
        }

        public GrandmastersRobe(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
