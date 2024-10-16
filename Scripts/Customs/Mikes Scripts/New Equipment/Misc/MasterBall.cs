using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class MasterBall : Item
    {
        [Constructable]
        public MasterBall() : base(0x1870) // You can change this item ID if you'd like.
        {
            Weight = 1.0;
            Hue = 1152; // A different color (purple) to distinguish it, but adjust as desired.
            Name = "Master Ball";
        }

        public MasterBall(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("It must be in your backpack to use.");
                return;
            }

            from.SendMessage("Select a creature to capture.");
            from.Target = new CaptureTarget(this);
        }

        private class CaptureTarget : Target
        {
            private readonly MasterBall m_MasterBall;

            public CaptureTarget(MasterBall masterBall) : base(10, false, TargetFlags.None)
            {
                m_MasterBall = masterBall;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (creature.Controlled || creature.IsBonded)
                    {
                        from.SendMessage("You can't capture tamed or bonded creatures!");
                        return;
                    }

                    creature.Controlled = true;
                    creature.ControlMaster = from;
                    from.SendMessage("You've captured the creature!");

                    m_MasterBall.Consume(); // Use the ball
                }
                else
                {
                    from.SendMessage("You can't capture that!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
