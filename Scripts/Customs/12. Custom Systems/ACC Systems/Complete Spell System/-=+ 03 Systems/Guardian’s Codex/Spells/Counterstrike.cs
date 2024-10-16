using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class Counterstrike : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Counterstrike", "Counterstrike!",
                                                        //SpellCircle.Fourth,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public Counterstrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Counterstrike m_Owner;

            public InternalTarget(Counterstrike owner) : base(2, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!m_Owner.Caster.CanSee(target))
                    {
                        m_Owner.Caster.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);
                        Effects.SendMovingEffect(m_Owner.Caster, target, 0x36E4, 7, 0, false, false, 0, 0); // Visual effect of flying weapon
                        m_Owner.Caster.PlaySound(0x29); // Sound effect of weapon swing

                        int damage = Utility.RandomMinMax(30, 50); // Heavy damage

                        target.Damage(damage, m_Owner.Caster);
                        target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist); // Blood splatter visual effect
                        target.PlaySound(0x1F2); // Impact sound
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
