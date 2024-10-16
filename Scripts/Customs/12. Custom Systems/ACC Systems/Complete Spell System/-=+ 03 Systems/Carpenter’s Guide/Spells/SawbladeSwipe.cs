using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class SawbladeSwipe : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sawblade Swipe", "Kal Saw Xen", 
            21004, // Sound ID
            9300   // Cast effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public SawbladeSwipe(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SawbladeSwipe m_Owner;

            public InternalTarget(SawbladeSwipe owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                {
                    m_Owner.Target((IPoint3D)o);
                }
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
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Define cone range and damage
                int coneRange = 5;
                double damage = Utility.RandomMinMax(15, 25); // Example damage range

                Effects.PlaySound(Caster.Location, Caster.Map, 0x11B0); // Sawblade sound effect

                // Visual effect for sawblades
                for (int i = 0; i < 5; i++) // Number of sawblades
                {
                    Point3D effectLocation = new Point3D(Caster.X + Utility.RandomMinMax(-coneRange, coneRange), Caster.Y + Utility.RandomMinMax(-coneRange, coneRange), Caster.Z);
                    Effects.SendLocationEffect(effectLocation, Caster.Map, 0x11B0, 30, 10, 0, 0);
                }

                // Damage enemies in a cone
                List<Mobile> targets = new List<Mobile>();
                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), coneRange);

                    foreach (Mobile m in eable)
                    {
                        if (Caster.CanBeHarmful(m) && Caster.InLOS(m) && m != Caster)
                        {
                            targets.Add(m);
                        }
                    }

                    eable.Free();
                }

                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);
                    AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0); // 100% physical damage

                    // Blood effect on hit
                    target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Waist);
                    target.PlaySound(0x11AC); // Sound effect on hit
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
