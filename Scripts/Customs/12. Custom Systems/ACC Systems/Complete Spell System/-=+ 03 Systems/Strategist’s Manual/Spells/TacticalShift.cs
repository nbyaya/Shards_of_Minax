using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class TacticalShift : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tactical Shift", "Move with Speed",
            //SpellCircle.Fourth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public TacticalShift(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Find a valid location to shift to within a certain range
                Point3D toLocation = GetRandomLocation(Caster.Location, Caster.Map, 4);

                if (toLocation != Point3D.Zero)
                {
                    // Play visual and sound effects
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE);

                    // Move the caster to the new location
                    Caster.MoveToWorld(toLocation, Caster.Map);

                    // Play visual and sound effects at new location
                    Effects.SendLocationParticles(EffectItem.Create(toLocation, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);
                    Effects.PlaySound(toLocation, Caster.Map, 0x1FE);

                    Caster.SendMessage("You swiftly reposition yourself to a more advantageous location!");
                }
                else
                {
                    Caster.SendMessage("There is no suitable location to shift to!");
                }
            }

            FinishSequence();
        }

        private Point3D GetRandomLocation(Point3D fromLocation, Map map, int range)
        {
            // Attempt to find a random location within the specified range
            for (int i = 0; i < 10; i++)
            {
                int x = fromLocation.X + Utility.RandomMinMax(-range, range);
                int y = fromLocation.Y + Utility.RandomMinMax(-range, range);
                int z = map.GetAverageZ(x, y);

                // Corrected: Use the appropriate overload of CanSpawnMobile
                if (map.CanSpawnMobile(x, y, z))
                {
                    return new Point3D(x, y, z);
                }
            }

            return Point3D.Zero; // No valid location found
        }

    }
}
