using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class ShadowBind : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Bind", "Sin Vas Grav",
            21014, // Icon ID for the spell
            9213,  // Icon hue for the spell
            false, // Not a harmful spell (will not cause aggression on use)
            Reagent.Nightshade,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public ShadowBind(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowBind m_Owner;

            public InternalTarget(ShadowBind owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(501857); // That cannot be harmed.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        SpellHelper.Turn(from, target);

                        // Apply visual and sound effects
                        Effects.SendLocationParticles(
                            EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                            0x376A, 10, 15, 1150, 4, 9502, 0
                        );
                        target.PlaySound(0x1FB);

                        // Apply the effect: Root and Silence the target
                        TimeSpan duration = TimeSpan.FromSeconds(5 + from.Skills[SkillName.Magery].Value / 10); // 5 seconds + 0.5 seconds per 10 magery skill

                        target.Paralyze(duration);
                        target.SendMessage("Shadowy tendrils bind you in place, preventing movement and speech!");

                        new InternalTimer(target, duration).Start();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501857); // That cannot be targeted.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Target;

            public InternalTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
            }

            protected override void OnTick()
            {
                if (m_Target != null && !m_Target.Deleted)
                {
                    m_Target.SendMessage("The shadowy tendrils release you.");
                    m_Target.Paralyzed = false;
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Slightly longer casting time
        }
    }
}
