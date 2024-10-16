using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.SkillHandlers;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class TurbulentWail : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Turbulent Wail", "Uus Hur",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        public TurbulentWail(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play the scream sound and add some visuals
                Caster.PlaySound(0x50F); // Sound of a scream
                Caster.FixedParticles(0x3779, 1, 15, 9909, 1153, 0, EffectLayer.Head); // Blue swirl effect

                // Define the radius of effect
                int radius = 5;
                int maxTeleportRadius = 8;

                // Find all mobiles (creatures and players) within a 5-tile radius
                IPooledEnumerable eable = Caster.GetMobilesInRange(radius);

                ArrayList targets = new ArrayList();

                foreach (Mobile m in eable)
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false) && m is BaseCreature)
                    {
                        targets.Add(m);
                    }
                }

                eable.Free();

                // Teleport each valid target to a random location within an 8-tile radius
                foreach (Mobile m in targets)
                {
                    // Calculate a random location within 8 tiles
                    Point3D from = m.Location;
                    Point3D to = GetRandomPoint(Caster.Location, maxTeleportRadius);

                    // Teleport the target
                    m.MoveToWorld(to, Caster.Map);
                    m.PlaySound(0x1FE); // Play a magical teleport sound
                    m.FixedParticles(0x376A, 10, 15, 5036, EffectLayer.Waist); // Visual effect of teleportation
                }

                Caster.SendMessage("Your powerful wail causes all nearby creatures to scatter!");
            }

            FinishSequence();
        }

        private Point3D GetRandomPoint(Point3D from, int maxRange)
        {
            // Generate a random point within maxRange tiles
            int x = from.X + Utility.RandomMinMax(-maxRange, maxRange);
            int y = from.Y + Utility.RandomMinMax(-maxRange, maxRange);
            int z = from.Z;

            // Adjust for valid map location
            Map map = Caster.Map;
            for (int i = 0; i < 10; i++) // Attempt to find a valid location within 10 tries
            {
                Point3D p = new Point3D(x, y, z);
                if (map.CanSpawnMobile(p))
                    return p;

                x = from.X + Utility.RandomMinMax(-maxRange, maxRange);
                y = from.Y + Utility.RandomMinMax(-maxRange, maxRange);
            }

            return from; // Fallback to original position if no valid point found
        }
    }
}
