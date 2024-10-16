using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class RapidFire : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rapid Fire", "Velox Ignis",
            21005, 9301,
            Reagent.BlackPearl, Reagent.SpidersSilk
        );

        public override SpellCircle Circle => SpellCircle.Sixth;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 25;

        public RapidFire(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the dexterity increase buff
                Caster.SendMessage("You feel a surge of agility coursing through your veins!");
                Caster.PlaySound(0x208); // A sound effect when the ability is activated
                Caster.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist); // A particle effect around the caster

                int dexBonus = (int)(Caster.Dex * 0.5); // Calculate 50% dexterity increase
                Caster.Dex += dexBonus; // Apply the dexterity bonus
                
                Timer.DelayCall(TimeSpan.FromSeconds(15.0), () =>
                {
                    Caster.Dex -= dexBonus; // Remove the dexterity bonus after 15 seconds
                    Caster.SendMessage("Your rapid fire ability fades, and your agility returns to normal.");
                    Caster.PlaySound(0x1F8); // Another sound effect when the ability ends
                    Caster.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist); // Another particle effect when the ability ends
                });
            }

            FinishSequence();
        }
    }
}
