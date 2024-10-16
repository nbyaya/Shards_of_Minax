using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class TacticalRetreat : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tactical Retreat", "Vas Rel Port",
            21014,
            9413,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 25; } }

        public TacticalRetreat(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play sound and visual effect at caster's location
                Caster.PlaySound(0x1FE); // Sound for a teleport spell
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044);

                // Teleport the caster to a random location within a 5-tile radius
                Point3D targetLocation = GetRandomRetreatLocation(Caster.Location, 5);

                // Apply invisibility effect for 5 seconds
                Caster.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(5), () => { Caster.Hidden = false; });

                // Move caster to the new location
                Caster.MoveToWorld(targetLocation, Caster.Map);

                // Play additional effects at the new location
                Effects.PlaySound(targetLocation, Caster.Map, 0x1FE);
                Effects.SendLocationParticles(EffectItem.Create(targetLocation, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044);

                Caster.SendMessage("You have swiftly retreated from combat!");
            }

            FinishSequence();
        }

        private Point3D GetRandomRetreatLocation(Point3D currentLocation, int range)
        {
            // Generate random offset within the specified range
            int offsetX = Utility.RandomMinMax(-range, range);
            int offsetY = Utility.RandomMinMax(-range, range);

            // Calculate new location
            Point3D newLocation = new Point3D(currentLocation.X + offsetX, currentLocation.Y + offsetY, currentLocation.Z);

            // Ensure the location is walkable
            if (Caster.Map.CanFit(newLocation, 16, true, false))
            {
                return newLocation;
            }

            // If the location isn't walkable, recursively try another location
            return GetRandomRetreatLocation(currentLocation, range);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
