using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class PrecisionStrike : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Precision Strike", // Spell name
            "Dexio Vas", // Invocation
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public PrecisionStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                ApplyEffect(Caster);
            }

            FinishSequence();
        }

        private void ApplyEffect(Mobile caster)
        {
            // Play sound effect
            caster.PlaySound(0x1F4); // Sound ID for "energy drain"

            // Create visual effect
            caster.FixedEffect(0x373A, 10, 16); // Visual effect for "energy drain"

            // Apply dexterity boost
            int boost = (int)(caster.Dex * 0.2);
            caster.Dex += boost;

            // Notify caster
            caster.SendMessage("Your dexterity has been boosted by 20%!");

            // Schedule removal of effect after 30 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveEffect(caster, boost));
        }

        private void RemoveEffect(Mobile caster, int boost)
        {
            if (caster != null && caster.Alive)
            {
                caster.Dex -= boost;
                caster.SendMessage("Your dexterity boost has worn off.");
            }
        }
    }
}
