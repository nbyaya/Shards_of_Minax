using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class Pokeball : Item
    {
        [Constructable]
        public Pokeball() : base(0x1870) // You can change this item ID if you want.
        {
            Weight = 1.0;
            Hue = 38; // Red color, you can adjust this.
            Name = "Pokeball";
        }

        public Pokeball(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("It must be in your backpack to use.");
                return;
            }

            from.SendMessage("Select a creature to try and capture.");
            from.Target = new CaptureTarget(this);
        }

        private class CaptureTarget : Target
        {
            private readonly Pokeball m_Pokeball;

            public CaptureTarget(Pokeball pokeball) : base(10, false, TargetFlags.None)
            {
                m_Pokeball = pokeball;
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

                    double baseCaptureRate = 0.3; // 30% base capture rate
                    double captureChance = baseCaptureRate;

                    captureChance += (from.Skills.AnimalTaming.Value * 0.015); // Increased multiplier
                    captureChance += ((creature.HitsMax - creature.Hits) / (double)creature.HitsMax) * 0.04; // Based on percentage of health lost with increased multiplier

                    // Limiting the chance to a value between 0 and 1 (0% to 100%)
                    captureChance = Math.Max(0, Math.Min(1, captureChance));

                    if (Utility.RandomDouble() <= captureChance)
                    {
                        creature.Controlled = true;
                        creature.ControlMaster = from;
                        from.SendMessage("You've captured the creature!");

                        m_Pokeball.Consume(); // Use the ball
                    }
                    else
                    {
                        from.SendMessage("The capture failed.");
                        m_Pokeball.Consume(); // Use the ball even if it fails
                    }
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
