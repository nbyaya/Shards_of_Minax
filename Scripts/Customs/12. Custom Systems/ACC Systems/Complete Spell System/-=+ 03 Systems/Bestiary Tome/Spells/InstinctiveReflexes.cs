using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class InstinctiveReflexes : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Instinctive Reflexes", "Flucto Defero",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9400
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 14; } }

        public InstinctiveReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Duration of the effect (30 seconds)
                TimeSpan duration = TimeSpan.FromSeconds(30.0);

                // Amount of Dexterity to increase (35% of current Dex)
                int dexIncrease = (int)(Caster.RawDex * 0.35);

                // Apply the effect
                Caster.RawDex += dexIncrease;
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F2); // Sound effect

                Timer.DelayCall(duration, () => 
                {
                    Caster.RawDex -= dexIncrease; // Revert Dex back to normal
                    Caster.SendMessage("The effects of Instinctive Reflexes have worn off.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
