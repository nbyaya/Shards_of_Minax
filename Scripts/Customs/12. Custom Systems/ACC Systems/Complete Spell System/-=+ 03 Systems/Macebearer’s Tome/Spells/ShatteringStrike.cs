using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class ShatteringStrike : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shattering Strike", "Mortum Defensor",
                                                        //SpellCircle.Fifth,
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public ShatteringStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShatteringStrike m_Owner;

            public InternalTarget(ShatteringStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Effects and Visuals
                        Effects.PlaySound(target.Location, target.Map, 0x1FB); // Sound for shattering effect
                        Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10, 0, 0); // Visual effect

                        // Apply debuff to target
                        TimeSpan duration = TimeSpan.FromSeconds(10.0 + (from.Skills[m_Owner.CastSkill].Value / 10.0));
                        target.SendMessage("Your defenses are shattered!");
                        target.PlaySound(0x307); // Additional sound for effect
                        target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Waist); // Additional visual effect

                        new ShatteringStrikeTimer(target, duration).Start();
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        private class ShatteringStrikeTimer : Timer
        {
            private Mobile m_Target;

            public ShatteringStrikeTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;

                // Reduce parry or block chance by 20%
                if (m_Target != null && !m_Target.Deleted)
                {
                    // Ensure we can access ParrySkill
                    m_Target.SendMessage("Your parry skill has been reduced!");
                    m_Target.Skills[SkillName.Parry].Base = Math.Max(0, m_Target.Skills[SkillName.Parry].Base - (m_Target.Skills[SkillName.Parry].Base * 0.2));
                }
            }

            protected override void OnTick()
            {
                if (m_Target != null && !m_Target.Deleted)
                {
                    m_Target.SendMessage("Your defenses have recovered.");
                }
            }
        }
    }
}
