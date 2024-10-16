using System;
using Server;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class MedalOfHonor : GoldBracelet
    {
        [Constructable]
        public MedalOfHonor()
        {
            Name = "Medal of Honor";
            Hue = 1157;  // Gold hue, you can adjust as needed
            Attributes.Luck = 1000; // Increases luck, which generally increases drop rates in UO. Adjust as needed.
        }

        public override bool OnEquip(Mobile from)
        {
            if (base.OnEquip(from))
            {
                from.SendMessage("You feel more fortunate wearing the Medal of Honor.");
                return true;
            }

            return false;
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is Mobile)
            {
                ((Mobile)parent).SendMessage("The blessing of the Medal of Honor fades.");
            }
        }

        public MedalOfHonor(Serial serial)
            : base(serial)
        {
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
