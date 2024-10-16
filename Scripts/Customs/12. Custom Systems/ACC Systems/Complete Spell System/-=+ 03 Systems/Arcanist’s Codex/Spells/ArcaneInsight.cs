using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ArcaneInsight : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Arcane Insight", "Revela Exsisto",
                                                        21005,
                                                        9408,
                                                        false
                                                       );
													  
        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }													  

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public ArcaneInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArcaneInsight m_Owner;

            public InternalTarget(ArcaneInsight owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                    m_Owner.Target(point);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Map map = Caster.Map;
                Point3D loc = new Point3D(p);

                Effects.PlaySound(loc, map, 0x29); // Sound effect for spell cast

                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044, 0, 0, 0); // Blue sparkles effect

                foreach (Mobile m in Caster.GetMobilesInRange(5)) // Radius of 5 tiles
                {
                    if (m.Hidden && Caster.CanSee(m))
                    {
                        m.RevealingAction();
                        m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head); // Flashy visual effect when a creature is revealed
                        m.PlaySound(0x1F5); // Sound effect when a creature is revealed
                    }
                }
            }

            FinishSequence();
        }
    }
}
