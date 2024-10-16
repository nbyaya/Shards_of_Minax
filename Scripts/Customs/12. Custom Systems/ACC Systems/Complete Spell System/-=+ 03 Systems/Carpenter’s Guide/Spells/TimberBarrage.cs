using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class TimberBarrage : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Timber Barrage", "TIMBER!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }  // Adjust this according to the skill tier you want
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 35; } }

        public TimberBarrage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Effects.PlaySound(loc, Caster.Map, 0x2F); // Wood creaking sound
                Effects.SendLocationEffect(loc, Caster.Map, 0x36D4, 30, 10, 1153, 0); // Falling logs visual

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => DamageEnemies(loc, Caster.Map));
            }

            FinishSequence();
        }

        private void DamageEnemies(Point3D location, Map map)
        {
            List<Mobile> toDamage = new List<Mobile>();

            foreach (Mobile m in map.GetMobilesInRange(location, 4))
            {
                if (Caster.CanBeHarmful(m) && Caster != m && m.Alive && !m.IsDeadBondedPet)
                    toDamage.Add(m);
            }

            foreach (Mobile m in toDamage)
            {
                Caster.DoHarmful(m);
                AOS.Damage(m, Caster, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0); // Adjust damage as needed
                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Log impact visual effect
                m.PlaySound(0x3D); // Log impact sound
            }
        }

        private class InternalTarget : Target
        {
            private TimberBarrage m_Owner;

            public InternalTarget(TimberBarrage owner) : base(12, true, TargetFlags.None)
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
