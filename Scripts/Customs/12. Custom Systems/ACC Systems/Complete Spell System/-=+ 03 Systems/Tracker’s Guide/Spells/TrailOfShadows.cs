using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class TrailOfShadows : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Trail of Shadows", "Umbral Tracitus",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public TrailOfShadows(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Effects.PlaySound(loc, Caster.Map, 0x1FB); // Play a shadowy sound effect
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1108, 0, 5042, 0); // Display shadowy smoke effects

                List<Point3D> shadowTrail = CreateTrail(loc);
                foreach (var point in shadowTrail)
                {
                    Effects.SendLocationEffect(point, Caster.Map, 0x3728, 10, 1, 0, 0); // Create a shadowy footprint effect
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => ClearEffect(point)); // Clear the effect after 5 seconds
                }

                // Apply tracking debuff to nearby enemies
                List<Mobile> enemies = GetEnemiesInRange(5);
                foreach (var enemy in enemies)
                {
                    ApplyTrackingDebuff(enemy);
                }
            }

            FinishSequence();
        }

        private List<Point3D> CreateTrail(Point3D start)
        {
            List<Point3D> trail = new List<Point3D>();
            for (int i = 0; i < 5; i++)
            {
                int xOffset = Utility.RandomMinMax(-2, 2);
                int yOffset = Utility.RandomMinMax(-2, 2);
                trail.Add(new Point3D(start.X + xOffset, start.Y + yOffset, start.Z));
            }
            return trail;
        }

        private List<Mobile> GetEnemiesInRange(int range)
        {
            List<Mobile> enemies = new List<Mobile>();
            foreach (Mobile m in Caster.GetMobilesInRange(range))
            {
                if (m != Caster && m.Alive && m.Combatant == Caster)
                    enemies.Add(m);
            }
            return enemies;
        }

        private void ApplyTrackingDebuff(Mobile enemy)
        {
            enemy.SendMessage("You are confused by the shadows and lose sight of your target.");
            enemy.AddStatMod(new StatMod(StatType.Dex, "TrailOfShadowsDebuff", -10, TimeSpan.FromSeconds(10.0))); // Reduces Dexterity temporarily
            enemy.FixedParticles(0x374A, 10, 15, 5029, EffectLayer.Head); // Display confusion particles above the enemy
            enemy.PlaySound(0x1FA); // Play a disorienting sound effect
        }

        private void ClearEffect(Point3D point)
        {
            Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 1, 0, 0); // Clear the shadow effect
        }

        private class InternalTarget : Target
        {
            private TrailOfShadows m_Owner;

            public InternalTarget(TrailOfShadows owner) : base(12, true, TargetFlags.None)
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
