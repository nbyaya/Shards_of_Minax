using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class BaitMastery : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Bait Mastery", "Lure Rare Fish",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public BaitMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Visual and sound effects
                    Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    Caster.PlaySound(0x1F5);

                    // Increase bait effectiveness
                    Caster.SendMessage("Your bait glows with a magical aura, attracting rare and valuable fish!");

                    // Logic to increase the chance of catching rare fish
                    double bonus = Caster.Skills[CastSkill].Value * 0.1;
                    double chance = Utility.RandomDouble() * 100;

                    if (chance < 20 + bonus)
                    {
                        Caster.SendMessage("You have attracted a rare fish!");
                        // Add logic to give the player a rare fish
                    }
                    else
                    {
                        Caster.SendMessage("You have attracted a valuable fish!");
                        // Add logic to give the player a valuable fish
                    }
                }
                catch
                {
                    Caster.SendMessage("The spell failed to cast.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
