using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class ReinforcedStructure : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Reinforced Structure", "Arma Fortis",
            21004, // Icon ID
            9300 // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public ReinforcedStructure(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (Caster.CanSee(m) && CheckSequence())
            {
                Caster.DoHarmful(m);
                SpellHelper.Turn(Caster, m);

                // Apply armor buff effect
                int duration = 10; // Duration of armor effect in seconds
                int armorBonus = 20; // Amount of temporary armor

                m.FixedParticles(0x375A, 1, 30, 9962, EffectLayer.Waist); // Visual effect around the target
                m.PlaySound(0x1F2); // Play sound effect



                m.SendMessage("Your armor has been temporarily reinforced!"); // Send message to the target
                m.VirtualArmorMod += armorBonus;

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    m.VirtualArmorMod -= armorBonus; // Remove armor after duration
                    m.SendMessage("The reinforced structure effect has worn off.");
                });
            }
            else
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ReinforcedStructure m_Owner;

            public InternalTarget(ReinforcedStructure owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
