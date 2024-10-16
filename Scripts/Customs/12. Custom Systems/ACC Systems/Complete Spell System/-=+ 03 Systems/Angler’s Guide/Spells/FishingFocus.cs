using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingFocus : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Fishing Focus", "Enhance your fishing skills for a short period, improving the chances of catching high-quality fish.",
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

        public FishingFocus(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of fishing prowess!");

                // Visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F5);

                // Timer to remove the effect after duration
                Timer.DelayCall(TimeSpan.FromMinutes(5), () =>
                {
                    Caster.SendMessage("The fishing focus effect has worn off.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
