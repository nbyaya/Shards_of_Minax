using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class CriticalHit : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Critical Hit", "Crit Vas Xon",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 25; } }

        public CriticalHit(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CriticalHit m_Owner;

            public InternalTarget(CriticalHit owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Massive Damage Calculation
                        double damage = from.Skills[SkillName.Magery].Value / 2.0 + Utility.RandomMinMax(30, 50);
                        AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                        // Visual and Sound Effects
                        Effects.SendTargetParticles(target, 0x36BD, 20, 10, 5044, EffectLayer.Head);
                        Effects.PlaySound(target.Location, target.Map, 0x307);

                        // Cooldown Timer
                        from.BeginAction(typeof(CriticalHit));
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => from.EndAction(typeof(CriticalHit)));
                    }
                }
                else
                {
                    from.SendMessage("You can only target living creatures.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
