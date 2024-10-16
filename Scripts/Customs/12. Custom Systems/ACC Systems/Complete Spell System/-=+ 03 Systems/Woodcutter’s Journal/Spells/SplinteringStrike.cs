using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class SplinteringStrike : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Splintering Strike", "Splintus Feritum",
            //SpellCircle.Third,
            21010,
            9310
        );

        public override SpellCircle Circle => SpellCircle.Third;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;

        public SplinteringStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                Caster.DoHarmful(target);
                Effects.SendTargetParticles(target, 0x36BD, 1, 13, 1160, 3, 9502, EffectLayer.Waist, 0); // Blood splatter effect
                Effects.PlaySound(target.Location, target.Map, 0x1E2); // Sound of a splintering strike

                target.SendMessage("You have been hit with a Splintering Strike! You are bleeding!");
                target.PlaySound(0x1E2); // Target hears the bleeding sound

                BleedTimer timer = new BleedTimer(target, Caster.Skills[SkillName.Lumberjacking].Value / 10); // Damage over time based on caster's skill
                timer.Start();

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private SplinteringStrike m_Owner;

            public InternalTarget(SplinteringStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private int m_Damage;
            private int m_Ticks;

            public BleedTimer(Mobile target, double skill) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Damage = (int)skill; // Damage is scaled based on caster's skill
                m_Ticks = 5; // The effect lasts for 5 ticks
            }

            protected override void OnTick()
            {
                if (m_Target.Alive && m_Ticks > 0)
                {
                    m_Target.Damage(m_Damage);
                    m_Target.FixedParticles(0x36BD, 1, 13, 1160, 3, 9502, EffectLayer.Waist, 0); // Repeated blood effect
                    m_Target.PlaySound(0x1E2); // Repeated bleeding sound
                    m_Ticks--;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
