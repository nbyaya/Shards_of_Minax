using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class Throw : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Throw", "SUPLEX!",
            //SpellCircle.Third,
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public Throw(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (Caster.CanSee(target) && CheckSequence())
            {
                // Apply heavy damage
                double damage = Utility.RandomMinMax(30, 50); // Random heavy damage between 30 and 50
                SpellHelper.Damage(this, target, damage, 0, 100, 0, 0, 0);

                // Teleport target to a random location within a 5-tile radius
                Point3D targetLocation = target.Location;
                Map targetMap = target.Map;

                if (targetMap != null)
                {
                    Point3D randomLocation = GetRandomLocation(targetLocation, targetMap, 5);
                    target.Location = randomLocation;

                    // Play flashy visuals and sounds
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, targetMap, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                    Effects.PlaySound(target.Location, targetMap, 0x1FE);

                    // Additional visual effect after teleport
                    Effects.SendLocationParticles(EffectItem.Create(randomLocation, targetMap, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042);
                    Effects.PlaySound(randomLocation, targetMap, 0x1FC);
                }
            }

            FinishSequence();
        }

        private Point3D GetRandomLocation(Point3D center, Map map, int radius)
        {
            for (int i = 0; i < 10; i++) // Attempt 10 times to find a valid location
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);

                Point3D randomLocation = new Point3D(x, y, z);

                if (map.CanFit(randomLocation, 16, true, false)) // Check if the location is valid
                    return randomLocation;
            }

            return center; // If no valid location is found, return the original location
        }

        private class InternalTarget : Target
        {
            private Throw m_Owner;

            public InternalTarget(Throw owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
