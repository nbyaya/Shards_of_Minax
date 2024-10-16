using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class FirebreathersFlask : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Firebreather's Flask", "Ignis Exhalo",
            //SpellCircle.Third,
            266,
            9040,
            Reagent.SulfurousAsh,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        public FirebreathersFlask(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x20C); // Fire breath sound

                double damage = 10 + (Caster.Skills[SkillName.Alchemy].Value * 0.5);

                foreach (Mobile m in AcquireIndirectTargets(p, 3)) // 3 tile cone
                {
                    Caster.DoHarmful(m);

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.PlaySound(0x208); // Burning sound

                    SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private FirebreathersFlask m_Owner;

            public InternalTarget(FirebreathersFlask owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}