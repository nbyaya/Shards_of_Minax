using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class AnvilOfFury : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Anvil of Fury", "An Vil Fury",
            21014, // Icon ID
            9310
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 5.0; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 40; } }

        public AnvilOfFury(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(new Point3D(p), Caster.Map, 0x2E6); // Anvil sound effect
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), Caster.Map, EffectItem.DefaultDuration), 0x37B9, 10, 20, 5052); // Flame Strike visual

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.Map.GetMobilesInRange(new Point3D(p), 2))
                {
                    if (Caster.CanBeHarmful(m, false) && Caster.InLOS(m))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 30);

                    AOS.Damage(m, Caster, damage, 0, 100, 0, 0, 0); // 100% fire damage
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x208);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AnvilOfFury m_Owner;

            public InternalTarget(AnvilOfFury owner) : base(12, true, TargetFlags.Harmful)
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
