using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class RockformShield : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rockform Shield", "Kal Vas Xen Shel",
            21001, // Spell icon
            9300 // Cast sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RockformShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply defensive bonuses and visual effects
                Caster.PlaySound(0x65A); // Rock formation sound
                Caster.FixedParticles(0x376A, 1, 15, 9909, 1109, 7, EffectLayer.Waist); // Rock effect around caster
                Caster.SendMessage("You encase yourself in a protective rock shield!");

                // Apply temporary buff
                int bonusDefense = 20; // Increase defense
                Caster.VirtualArmorMod += bonusDefense;

                Timer.DelayCall(TimeSpan.FromSeconds(10), () => RemoveRockformShield(bonusDefense));
            }

            FinishSequence();
        }

        private void RemoveRockformShield(int bonusDefense)
        {
            Caster.VirtualArmorMod -= bonusDefense;

            Caster.PlaySound(0x658); // Sound when shield ends
            Caster.FixedParticles(0x376A, 1, 15, 9909, 1109, 7, EffectLayer.Waist); // Visual effect for shield removal
            Caster.SendMessage("Your rock shield crumbles away.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
