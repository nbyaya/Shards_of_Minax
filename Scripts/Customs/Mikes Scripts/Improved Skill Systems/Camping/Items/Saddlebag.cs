using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class Saddlebag : Item
    {
        [Constructable]
        public Saddlebag() : base(0x9B2) // Backpack graphic
        {
            Name = "a saddlebag";
            Weight = 1.0;
            Hue = 0; // You can set a custom hue here
        }

        public Saddlebag(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("That must be in your backpack to use it.");
                return;
            }

            from.SendMessage("Target a tamed horse or llama to convert it into a pack animal.");
            from.Target = new SaddlebagTarget(this);
        }

        private class SaddlebagTarget : Target
        {
            private Saddlebag m_Saddlebag;

            public SaddlebagTarget(Saddlebag saddlebag) : base(10, false, TargetFlags.None)
            {
                m_Saddlebag = saddlebag;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Saddlebag == null || m_Saddlebag.Deleted)
                    return;

                if (targeted is BaseCreature creature)
                {
                    if (!creature.Controlled || creature.ControlMaster != from)
                    {
                        from.SendMessage("That creature must be tamed and under your control.");
                        return;
                    }

                    BaseCreature newCreature = null;

                    if (creature is Horse)
                    {
                        newCreature = new PackHorse();
                    }
                    else if (creature is Llama)
                    {
                        newCreature = new PackLlama();
                    }

                    if (newCreature != null)
                    {
                        newCreature.Controlled = true;
                        newCreature.ControlMaster = from;
                        newCreature.ControlOrder = creature.ControlOrder;

                        newCreature.MoveToWorld(creature.Location, creature.Map);
                        creature.Delete();

                        from.SendMessage("The creature has been converted into a pack animal.");
                        m_Saddlebag.Delete();
                    }
                    else
                    {
                        from.SendMessage("That creature cannot be converted.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }
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
