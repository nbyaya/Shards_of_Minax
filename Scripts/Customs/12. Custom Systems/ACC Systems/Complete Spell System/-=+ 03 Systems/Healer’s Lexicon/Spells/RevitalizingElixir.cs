using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class RevitalizingElixir : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                    "Revitalizing Elixir", "Elixus Revitae",
                                                    21004,
                                                    9300
                                                   );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public RevitalizingElixir(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RevitalizingElixir m_Owner;

            public InternalTarget(RevitalizingElixir owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;
                    if (m_Owner.CheckSequence())
                    {
                        // Play sound and visual effect for potion creation
                        from.PlaySound(0x207); // Sound for brewing
                        Effects.SendLocationEffect(from.Location, from.Map, 0x373A, 20, 10, 1160, 0); // Bright potion brewing effect

                        // Create the potion
                        RevitalizingElixirPotion potion = new RevitalizingElixirPotion();
                        from.AddToBackpack(potion);

                        from.SendMessage("You have created a Revitalizing Elixir!");

                        // Finish the casting sequence
                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendMessage("You can only create the elixir for yourself.");
                }
            }
        }

        public class RevitalizingElixirPotion : Item
        {
            [Constructable]
            public RevitalizingElixirPotion() : base(0xF0B) // Potion bottle graphic
            {
                Name = "Revitalizing Elixir";
                Hue = 1153; // Light green hue for a healing potion
                Weight = 1.0;
                Stackable = true;
            }

            public RevitalizingElixirPotion(Serial serial) : base(serial)
            {
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (!Movable)
                    return;

                if (from.InRange(this.GetWorldLocation(), 1))
                {
                    // Apply immediate health boost
                    int healthBoost = Utility.RandomMinMax(30, 50);
                    from.Hits += healthBoost;
                    from.SendMessage("You feel a surge of vitality as you drink the elixir.");

                    // Start a healing over time effect
                    new RevitalizingElixirHealTimer(from).Start();

                    // Play sound and visual effect for consuming the potion
                    from.PlaySound(0x1F9); // Drinking sound
                    Effects.SendTargetEffect(from, 0x376A, 10, 10, 1153, 0); // Green healing effect

                    // Consume the potion
                    this.Delete();
                }
                else
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
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

        private class RevitalizingElixirHealTimer : Timer
        {
            private Mobile m_Target;
            private int m_Ticks;

            public RevitalizingElixirHealTimer(Mobile target) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Ticks = 0;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_Ticks++;

                // Heal a small amount each tick
                int healAmount = Utility.RandomMinMax(2, 5);
                m_Target.Hits += healAmount;
                m_Target.SendMessage("The elixir continues to heal you over time.");
                Effects.SendTargetEffect(m_Target, 0x376A, 10, 10, 1153, 0); // Green healing effect

                if (m_Ticks >= 10) // Duration of healing over time
                {
                    Stop();
                }
            }
        }
    }
}
