using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class UnderwaterBreathing : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Underwater Breathing", "Aqua Vitae",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public UnderwaterBreathing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x64F); // Play a water-related sound
                Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Create water splash effect

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (Caster.CanBeBeneficial(m, false))
                    {
                        Caster.DoBeneficial(m);
                        m.SendMessage("You feel a surge of energy allowing you to breathe underwater!");
                        m.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Create water splash effect on allies
                        m.PlaySound(0x64F); // Play a water-related sound for allies

                        // Apply the underwater breathing effect
                        m.AddStatMod(new StatMod(StatType.Dex, "UnderwaterBreathing", 10, TimeSpan.FromMinutes(5)));
                    }
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
