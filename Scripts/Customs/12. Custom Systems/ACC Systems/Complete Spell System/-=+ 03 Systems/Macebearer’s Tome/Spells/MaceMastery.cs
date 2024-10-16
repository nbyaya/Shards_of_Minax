using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class MaceMastery : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mace Mastery", "Maestria di Mace",
            21004, // Icon
            9300   // Sound effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public MaceMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster is PlayerMobile player)
            {
                // Check if the player is using a mace by checking if the weapon is a subclass of BaseWeapon
                if (player.Weapon is BaseWeapon weapon && IsMace(weapon))
                {
                    ApplyBuff(player);
                }
                else
                {
                    Caster.SendMessage("You need to equip a mace to benefit from Mace Mastery.");
                }
            }

            FinishSequence();
        }

        private bool IsMace(BaseWeapon weapon)
        {
            // Example implementation: Check for known mace types
            return weapon is Mace || weapon is Club || weapon is Maul || weapon is WarMace;
            // Add other mace types as needed
        }

        private void ApplyBuff(PlayerMobile player)
        {
            // Example implementation, adjust based on available properties or methods
            // Increase accuracy and critical hit chance (replace with actual available methods)
            player.SendMessage("You feel your mastery over maces empowering you with increased accuracy and critical strike chance!");

            // Visual and sound effect for applying the buff
            Effects.PlaySound(player.Location, player.Map, 0x5C2); // Sound effect for a buff
            player.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist); // Visual effect for the buff

            // Buff duration and removal timer
            Timer.DelayCall(TimeSpan.FromMinutes(2.0), () => RemoveBuff(player)); // Duration of 2 minutes
        }

        private void RemoveBuff(PlayerMobile player)
        {
            // Notify the player
            player.SendMessage("Your Mace Mastery effect has worn off.");
        }
    }
}
