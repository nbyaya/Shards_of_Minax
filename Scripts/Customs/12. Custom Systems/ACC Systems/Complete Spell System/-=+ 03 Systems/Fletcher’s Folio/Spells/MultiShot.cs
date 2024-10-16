using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class MultiShot : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Multi-Shot", "Multi-Shot",
                                                        21005,
                                                        9400,
                                                        false
                                                       );

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public MultiShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Determine the cone's angle and range
                double coneAngle = Math.PI / 4; // 45 degrees in radians
                int range = 10;

                // Play sound and animation
                Caster.PlaySound(0x145); // Play bow sound
                Caster.Animate(9, 1, 1, true, false, 0); // Shooting animation

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(range))
                {
                    if (m != Caster && Caster.InLOS(m) && IsInCone(Caster, m, coneAngle))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile target in targets)
                {
                    // Calculate damage
                    double damage = Utility.RandomMinMax(10, 20);
                    damage *= 1.0 + (Caster.Skills[SkillName.Archery].Value / 100.0); // Increase damage based on skill

                    // Apply damage
                    AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0); // 100% physical damage

                    // Apply visual effect
                    Effects.SendMovingEffect(Caster, target, 0xF42, 18, 1, false, false, 0, 0);
                }
            }

            FinishSequence();
        }

        private bool IsInCone(Mobile caster, Mobile target, double coneAngle)
        {
            double angleToTarget = Math.Atan2(target.Y - caster.Y, target.X - caster.X);
            double casterDirectionRadians = DirectionToRadians(caster.Direction);
            double angleDifference = Math.Abs(angleToTarget - casterDirectionRadians);

            return angleDifference <= coneAngle / 2;
        }

        private double DirectionToRadians(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return 0;
                case Direction.Right: return Math.PI / 4;
                case Direction.East: return Math.PI / 2;
                case Direction.Down: return 3 * Math.PI / 4;
                case Direction.South: return Math.PI;
                case Direction.Left: return 5 * Math.PI / 4;
                case Direction.West: return 3 * Math.PI / 2;
                case Direction.Up: return 7 * Math.PI / 4;
                default: return 0;
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
