using System;
using Server;

namespace Server.Items
{
    public class PlagueArmor : BoneChest // or use LeatherChest/BoneArms/etc
    {
        [Constructable]
        public PlagueArmor()
        {
            Name = "Plaguebound Armor";
            Hue = 0x455;
            Attributes.BonusHits = 5;
            Attributes.RegenHits = -2; // Slightly cursed
            Attributes.LowerManaCost = 5;
        }

        public PlagueArmor(Serial serial) : base(serial)
        {
        }

        public override bool OnEquip(Mobile from)
        {
            
            from.SendMessage("A chill creeps across your skin as you don the plague-touched armor.");
			return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is Mobile m)
                m.SendMessage("The air clears around you as you remove the cursed armor.");
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
