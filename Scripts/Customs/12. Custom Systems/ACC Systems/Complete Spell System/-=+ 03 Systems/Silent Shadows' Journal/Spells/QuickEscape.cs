using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class QuickEscape : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Escape", "Nothing personal kid",
            //SpellCircle.Fifth,
            21006,
            9205
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public QuickEscape(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Point3D teleportTarget = GetRandomLocation(Caster.Location, Caster.Map, 10);

                if (teleportTarget != Point3D.Zero)
                {
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE); // Sound effect for teleport start

                    Caster.Location = teleportTarget;
                    Caster.ProcessDelta();

                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE); // Sound effect for teleport end
                }
                else
                {
                    Caster.SendMessage("You failed to find a suitable location to teleport to.");
                }
            }

            FinishSequence();
        }

        private Point3D GetRandomLocation(Point3D start, Map map, int range)
        {
            // Attempt to find a random, valid location within the specified range
            for (int i = 0; i < 10; i++)
            {
                int x = start.X + Utility.RandomMinMax(-range, range);
                int y = start.Y + Utility.RandomMinMax(-range, range);
                int z = map.GetAverageZ(x, y);

                Point3D point = new Point3D(x, y, z);

                if (map.CanSpawnMobile(point) && !SpellHelper.CheckMulti(point, map))
                    return point;
            }

            return Point3D.Zero; // No valid location found
        }
    }
}
