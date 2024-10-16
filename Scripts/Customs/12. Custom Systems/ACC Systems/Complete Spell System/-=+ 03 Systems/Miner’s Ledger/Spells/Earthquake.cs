using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class Earthquake : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Earthquake", "Terra Tremor",
                                                        21012,
                                                        9300,
                                                        false,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 40; } }

        public Earthquake(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x2F3); // Earthquake sound
                Effects.SendLocationEffect(p, Caster.Map, 0x36B0, 30, 10, 0x22, 0); // Tremor visual effect

                ArrayList targets = new ArrayList();

                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(new Point3D(p), 3);
                foreach (Mobile m in eable)
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }
                eable.Free();

                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);

                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(m, Caster, damage, 100, 0, 0, 0, 0); // Pure physical damage

                    // Chance to knock down
                    if (Utility.RandomDouble() < 0.25) // 25% chance
                    {
                        m.Animate(21, 6, 1, true, false, 0); // Knockdown animation
                        m.Frozen = true;
                        Timer.DelayCall(TimeSpan.FromSeconds(2.0), () => m.Frozen = false); // Unfreeze after 2 seconds
                    }
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private Earthquake m_Owner;

            public InternalTarget(Earthquake owner) : base(12, true, TargetFlags.Harmful)
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
