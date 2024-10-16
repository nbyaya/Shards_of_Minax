using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class OreSense : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ore Sense", "Detus Ore",
            21003,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public OreSense(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                // Play a sound and create a visual effect at the caster's location
                Effects.PlaySound(loc, Caster.Map, 0x5C3);
                Effects.SendLocationEffect(loc, Caster.Map, 0x376A, 20, 10, 1161, 0); // Blue sparkle effect

                List<Point3D> nodes = FindOreNodes(Caster.Location, Caster.Map, 6); // Finds ore nodes within 6 tiles

                foreach (Point3D node in nodes)
                {
                    // Play sound and create visual effect at each ore node location
                    Effects.PlaySound(node, Caster.Map, 0x2E5);
                    Effects.SendLocationEffect(node, Caster.Map, 0x376A, 20, 10, 1161, 0); // Blue sparkle effect
                    // Optionally, you could spawn temporary ore items or creatures representing ore veins here
                }
            }

            FinishSequence();
        }

        private List<Point3D> FindOreNodes(Point3D center, Map map, int radius)
        {
            List<Point3D> oreNodes = new List<Point3D>();

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Point3D p = new Point3D(center.X + x, center.Y + y, center.Z);

                    if (Utility.InRange(center, p, radius) && map.CanSpawnMobile(p))
                    {
                        // You could add more logic here to check for specific ore types or conditions
                        oreNodes.Add(p);
                    }
                }
            }

            return oreNodes;
        }

        private class InternalTarget : Target
        {
            private OreSense m_Owner;

            public InternalTarget(OreSense owner) : base(12, true, TargetFlags.None)
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
