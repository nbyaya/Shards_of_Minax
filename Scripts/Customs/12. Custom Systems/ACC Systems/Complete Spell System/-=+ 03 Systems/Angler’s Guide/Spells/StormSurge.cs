using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class StormSurge : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Storm Surge", "Create a localized storm that deals electrical damage to enemies and increases fishing success.",
            266, // Animation ID
            9300 // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public StormSurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play sound at caster's location
                Effects.PlaySound(Caster.Location, Caster.Map, 0x5D9); // Thunder sound

                // Create storm effect at caster's location
                CreateStormEffect(Caster.Location, Caster.Map);

                // Apply electrical damage to nearby enemies
                ApplyElectricalDamage(Caster.Location, Caster.Map);

                // Increase fishing success
                IncreaseFishingSuccess(Caster);

                FinishSequence();
            }
        }

        private void CreateStormEffect(Point3D location, Map map)
        {
            // Create a storm visual effect
            Effects.SendLocationEffect(new Point3D(location.X, location.Y, location.Z + 10), map, 0x3709, 30, 10);
            Effects.SendLocationEffect(new Point3D(location.X, location.Y, location.Z + 10), map, 0x3709, 30, 10);
        }

        private void ApplyElectricalDamage(Point3D location, Map map)
        {
            foreach (Mobile m in map.GetMobilesInRange(location, 5))
            {
                if (m.Alive && m != Caster && Caster.CanBeHarmful(m))
                {
                    Caster.DoHarmful(m);
                    int damage = Utility.RandomMinMax(10, 20); // Random electrical damage
                    m.Damage(damage, Caster);

                    // Play electrical sound effect
                    Effects.SendTargetEffect(m, 0x4A, 10, 5, 0x2F, 0);
                }
            }
        }

        private void IncreaseFishingSuccess(Mobile caster)
        {
            if (caster is PlayerMobile player)
            {

                caster.SendMessage("Your fishing skill has been increased slightly due to the storm's influence.");
                
            }
        }
    }
}
