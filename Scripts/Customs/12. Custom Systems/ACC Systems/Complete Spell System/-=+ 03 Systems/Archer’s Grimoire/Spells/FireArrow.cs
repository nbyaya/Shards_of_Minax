using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class FireArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Fire Arrow", "In Flam Ylem",
                                                        // SpellCircle.Third,  // Uncomment if needed for other systems
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public FireArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FireArrow m_Owner;

            public InternalTarget(FireArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile m = (Mobile)target;

                    if (m_Owner.CheckHSequence(m))
                    {
                        SpellHelper.Turn(from, m);

                        // Visual effect for the arrow firing
                        Effects.SendMovingEffect(from, m, 0x36E4, 7, 0, false, false, 0x481, 0);

                        // Play a sound when the arrow is fired
                        from.PlaySound(0x208);

                        // Apply the burn effect after the arrow hits
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => ApplyFireEffect(from, m));

                        m_Owner.FinishSequence();
                    }
                }
            }

            private void ApplyFireEffect(Mobile caster, Mobile target)
            {
                if (target.Alive && !target.IsDeadBondedPet)
                {
                    target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    target.PlaySound(0x208);

                    int damage = Utility.RandomMinMax(10, 15); // Base damage

                    // Apply burn damage over time effect
                    new BurnEffectTimer(caster, target, damage).Start();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class BurnEffectTimer : Timer
        {
            private Mobile m_Caster;
            private Mobile m_Target;
            private int m_Damage;
            private int m_Ticks = 0;

            public BurnEffectTimer(Mobile caster, Mobile target, int damage) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(2.0))
            {
                m_Caster = caster;
                m_Target = target;
                m_Damage = damage;
            }

            protected override void OnTick()
            {
                if (m_Ticks >= 5 || m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                if (m_Target.Alive && !m_Target.IsDeadBondedPet)
                {
                    m_Target.Damage(m_Damage, m_Caster);
                    m_Target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m_Target.PlaySound(0x208);
                    m_Ticks++;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
