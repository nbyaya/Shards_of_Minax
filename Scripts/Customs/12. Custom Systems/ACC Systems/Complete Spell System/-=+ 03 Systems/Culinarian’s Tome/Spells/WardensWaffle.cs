using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class WardensWaffle : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Warden's Waffle", "Summon Waffle",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public WardensWaffle(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Summon the consumable item
                WardenWaffleItem waffle = new WardenWaffleItem();
                waffle.MoveToWorld(Caster.Location, Caster.Map);

                // Play a sound effect and visual effect
                Caster.PlaySound(0x2E6);
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 20, 10, 1153, 0);

                Caster.SendMessage("You have summoned a Warden's Waffle!");
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        private class WardenWaffleItem : Item
        {
            public override string DefaultName => "Warden's Waffle";
			
			[Constructable]
            public WardenWaffleItem() : base(0x1040) // Example item ID for a waffle
            {
                Hue = 1153; // Set a unique color for the waffle
                Weight = 1.0;
                Movable = true;
            }

            public WardenWaffleItem(Serial serial) : base(serial)
            {
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (from.InRange(this.GetWorldLocation(), 1))
                {
                    from.Hits += Utility.RandomMinMax(10, 30); // Restore random amount of health
                    from.SendMessage("You eat the Warden's Waffle and feel reinvigorated!");
                    Effects.SendTargetEffect(from, 0x373A, 10, 15, 0, 0);
                    from.PlaySound(0x52); // Eating sound effect

                    this.Delete(); // Remove the item after consumption
                }
                else
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
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
}
