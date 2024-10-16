using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class AdrenalineRush : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Adrenaline Rush", "Rush of Power",
                                                        // SpellCircle.Second,
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public AdrenalineRush(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the effects
                Caster.SendMessage("You feel a surge of power coursing through your veins!");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x1F5);

                // Buff Strength and Dexterity by 30%
                int originalStr = Caster.Str;
                int originalDex = Caster.Dex;

                int bonusStr = (int)(originalStr * 0.3);
                int bonusDex = (int)(originalDex * 0.3);

                Caster.RawStr += bonusStr;
                Caster.RawDex += bonusDex;

                // Apply a timer to remove the buff after a short duration
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    // Revert to original stats
                    Caster.RawStr = originalStr;
                    Caster.RawDex = originalDex;

                    Caster.SendMessage("The effects of the Adrenaline Rush have worn off.");
                    Caster.FixedParticles(0x3735, 10, 15, 5018, EffectLayer.Waist);
                    Caster.PlaySound(0x1F8);
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
