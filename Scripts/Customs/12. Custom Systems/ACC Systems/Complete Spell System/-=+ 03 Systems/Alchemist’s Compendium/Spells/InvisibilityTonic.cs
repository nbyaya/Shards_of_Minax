using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server;
using Server.Items;


namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class InvisibilityTonic : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Invisibility Tonic", "An Lor Xen",
            21005,
            9301,
            false,
            Reagent.Bloodmoss,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 25; } }

        public InvisibilityTonic(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private InvisibilityTonic m_Owner;

            public InternalTarget(InvisibilityTonic owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckBSequence(target))
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);
                        target.Hidden = true; // Make the target invisible

                        // Send invisibility particles
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        target.PlaySound(0x3C4); // Invisibility sound

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            if (target.Hidden)
                            {
                                target.RevealingAction();
                                // Send reveal particles
                                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                                target.PlaySound(0x1FD); // Reveal sound
                            }
                        });
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
