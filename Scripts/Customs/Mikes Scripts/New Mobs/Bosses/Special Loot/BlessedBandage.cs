using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
    public class BlessedBandage : Item
    {
        [Constructable]
        public BlessedBandage() : base(0xE21)
        {
            Movable = true;
            Hue = 0x48E;
            Name = "Blessed Bandage";
        }

        public BlessedBandage(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                if (from.Target == null)
                {
                    from.SendMessage("Who do you want to use the bandage on?");
                    from.Target = new InternalTarget(this);
                }
            }
            else
            {
                from.SendLocalizedMessage(500295); // You are too far away to do that.
            }
        }

        private class InternalTarget : Target
        {
            private BlessedBandage m_Bandage;

            public InternalTarget(BlessedBandage bandage) : base(2, false, TargetFlags.Beneficial)
            {
                m_Bandage = bandage;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeBeneficial(target, true))
                    {
                        from.SendMessage("You apply the blessed bandage.");

                        target.Heal(Utility.RandomMinMax(70, 90)); // Heal a random amount between 70 and 90

                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.PlaySound(0x1F2);

                        m_Bandage.Delete();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500970); // You cannot use that on that.
                }
            }
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
