using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class LightningStorm : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lightning Storm", "Kal Vas Xen",
            //SpellCircle.Sixth,
            21004,
            9300,
            false,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 35; } }

        public LightningStorm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);
                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                Map map = Caster.Map;

                if (map != null)
                {
                    // Display initial lightning strike effect
                    Effects.SendLocationEffect(loc, map, 0x2A4E, 30, 10, 0, 0);
                    Effects.PlaySound(loc, map, 0x29);

                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => CreateLightningStorm(loc, map, Caster));
                }
            }

            FinishSequence();
        }

        private void CreateLightningStorm(Point3D loc, Map map, Mobile caster)
        {
            if (map == null || caster == null)
                return;

            int bolts = 10 + (int)(caster.Skills[SkillName.Magery].Value / 10); // Number of lightning bolts

            for (int i = 0; i < bolts; i++)
            {
                Point3D target = new Point3D(loc.X + Utility.RandomMinMax(-3, 3), loc.Y + Utility.RandomMinMax(-3, 3), loc.Z);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () => StrikeLightning(target, map, caster));
            }
        }

        private void StrikeLightning(Point3D target, Map map, Mobile caster)
        {
            if (map == null)
                return;

            Effects.SendLocationEffect(target, map, 0x3967, 30, 10, 0, 0);
            Effects.PlaySound(target, map, 0x29);

            IPooledEnumerable eable = map.GetMobilesInRange(target, 1);

            foreach (Mobile m in eable)
            {
                if (m != null && caster.CanBeHarmful(m))
                {
                    caster.DoHarmful(m);
                    AOS.Damage(m, caster, Utility.RandomMinMax(10, 30), 0, 0, 0, 0, 100); // 100% energy damage
                    m.FixedEffect(0x3779, 10, 30);
                }
            }

            eable.Free();
        }

        private class InternalTarget : Target
        {
            private LightningStorm m_Owner;

            public InternalTarget(LightningStorm owner) : base(12, true, TargetFlags.Harmful)
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
