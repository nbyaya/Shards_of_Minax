using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class FlavorOfFury : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flavor of Fury", "In Tastus",
            21005,
            9301
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 25.0;
        public override int RequiredMana => 15;

        public FlavorOfFury(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of fury course through your veins!");
                Caster.PlaySound(0x1E3); // Play a dramatic sound effect

                // Temporary buff for Strength and Dexterity
                int strIncrease = (int)(Caster.Str * 0.15);
                int dexIncrease = (int)(Caster.Dex * 0.15);

                Caster.RawStr += strIncrease;
                Caster.RawDex += dexIncrease;

                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Play a fiery particle effect

                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    // Revert the buff after 10 seconds
                    Caster.RawStr -= strIncrease;
                    Caster.RawDex -= dexIncrease;

                    Caster.SendMessage("The fury fades, leaving you feeling normal again.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
