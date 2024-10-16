using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;
using Server;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class PiercingShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Piercing Shot", "Kal An Flam",
            21005, 9301, // Icon and Effect
            false, Reagent.BlackPearl // Example Reagent
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public PiercingShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Point3D loc = new Point3D(p);

                // Use IEntity for point target; Mobile or Item can also be used if needed
                IEntity targetEntity = new Entity(Serial.Zero, loc, Caster.Map);

                Effects.SendMovingEffect(Caster, targetEntity, 0xF42, 10, 0, false, false, 1161, 0); // Arrow effect
                Caster.PlaySound(0x145); // Arrow sound

                List<Mobile> targets = new List<Mobile>();
                Map map = Caster.Map;

                if (map != null)
                {
                    // Check for mobiles in line
                    foreach (Mobile m in map.GetMobilesInRange(Caster.Location, 20)) // Adjust range as needed
                    {
                        if (m != Caster && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                        {
                            if (IsInLine(Caster.Location, loc, m.Location))
                            {
                                targets.Add(m);
                            }
                        }
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile m in targets)
                    {
                        Caster.DoHarmful(m);
                        m.Damage(Utility.RandomMinMax(15, 30), Caster); // Random damage
                        m.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist); // Impact visual
                        m.PlaySound(0x1F1); // Impact sound
                    }
                }
            }

            FinishSequence();
        }

        private bool IsInLine(Point3D start, Point3D end, Point3D point)
        {
            // Simple line check
            double distance = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            double d1 = Math.Sqrt(Math.Pow(point.X - start.X, 2) + Math.Pow(point.Y - start.Y, 2));
            double d2 = Math.Sqrt(Math.Pow(point.X - end.X, 2) + Math.Pow(point.Y - end.Y, 2));

            return Math.Abs(d1 + d2 - distance) < 1.0; // Tolerance for floating point errors
        }

        private class InternalTarget : Target
        {
            private PiercingShot m_Owner;

            public InternalTarget(PiercingShot owner) : base(10, true, TargetFlags.Harmful)
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
