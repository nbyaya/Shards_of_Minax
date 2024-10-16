using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class ShadowMeld : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shadow Meld", "In Umbra",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.Nightshade,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 10; } }

        public ShadowMeld(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden)
            {
                Caster.SendLocalizedMessage(1061922); // You are already hidden.
                FinishSequence();
                return;
            }

            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 10, 15, 5038, EffectLayer.Waist);
                Caster.PlaySound(0x482); // Play a shadowy sound effect

                Caster.Hidden = true;
                Caster.SendLocalizedMessage(1061924); // You blend into the shadows, becoming harder to detect.

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    if (Caster.Hidden)
                    {
                        Caster.Hidden = false;
                        Caster.SendLocalizedMessage(1061923); // You emerge from the shadows.
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        // Remove the Target related code

        // You can also remove the InternalTarget class if it's not needed.
        private class InternalTarget : Target
        {
            private ShadowMeld m_Owner;

            public InternalTarget(ShadowMeld owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                // Removed the Target method call
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
