using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class QuickReflexes : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Reflexes", "Quick Reflexes!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(10.0); // Duration of the dexterity buff
        private const double DexBonusMultiplier = 0.4; // 40% increase in dexterity

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x5C); // Sound effect for spell cast
                Caster.FixedParticles(0x376A, 1, 29, 9964, 92, 3, EffectLayer.Waist); // Visual effect

                int originalDex = Caster.Dex; // Store original dexterity
                int dexBonus = (int)(originalDex * DexBonusMultiplier); // Calculate dexterity bonus

                Caster.Dex += dexBonus; // Apply dexterity bonus

                // Start a timer to remove the dexterity bonus after the duration expires
                Timer.DelayCall(BuffDuration, () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Dex -= dexBonus; // Remove dexterity bonus
                        Caster.SendMessage("Your quick reflexes have worn off."); // Notify player
                    }
                });

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Delay between casts
        }
    }
}
