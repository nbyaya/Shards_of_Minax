using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class SnoopersMasterScope : Item
    {
        [Constructable]
        public SnoopersMasterScope() : base(0x14F5)
        {
            Weight = 1.0;
            Name = "Snooper's Master Scope";
            Hue = 1153; // You can change this to any hue you prefer.
        }

        public SnoopersMasterScope(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.Snooping].Value < 150)
            {
                from.SendMessage("You need at least 150 snooping skill to use this.");
                return;
            }

            from.SendMessage("Which container do you want to snoop?");
            from.Target = new SnoopTarget();
        }

        private class SnoopTarget : Target
        {
            public SnoopTarget() : base(10, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Container)
                {
                    Container container = (Container)targeted;
                    container.DisplayTo(from);                    
                }
                else
                {
                    from.SendMessage("That is not a container.");
                }
            }
        }
    }
}
