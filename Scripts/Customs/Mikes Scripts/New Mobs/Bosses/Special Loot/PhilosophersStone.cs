using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class PhilosophersStone : Item
    {
        [Constructable]
        public PhilosophersStone() : base(0xF8E)
        {
            Weight = 1.0;
            Hue = 0x489;
            Name = "Philosopher's Stone";
        }

        public PhilosophersStone(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("Target the metal item you wish to transmute into gold.");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PhilosophersStone m_Stone;

            public InternalTarget(PhilosophersStone stone) : base(1, false, TargetFlags.None)
            {
                m_Stone = stone;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseIngot)
                {
                    BaseIngot ingot = (BaseIngot)targeted;

                    if (ingot.Amount >= 5)
                    {
                        ingot.Amount -= 5;
                        from.AddToBackpack(new GoldIngot(5));
                        from.SendMessage("You successfully transmute the metal into gold!");
                        from.PlaySound(0x1FF);
                        from.FixedEffect(0x376A, 10, 15);
                    }
                    else
                    {
                        from.SendMessage("You need at least 5 ingots to transmute into gold.");
                    }
                }
                else
                {
                    from.SendMessage("You can only transmute metal ingots into gold.");
                }
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