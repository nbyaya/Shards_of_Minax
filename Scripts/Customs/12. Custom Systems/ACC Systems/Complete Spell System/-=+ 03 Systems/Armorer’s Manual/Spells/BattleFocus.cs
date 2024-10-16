using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class BattleFocus : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Battle Focus", "Fortis Virtus Mens",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public BattleFocus(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1E9); // Play a power-up sound
                Caster.FixedParticles(0x375A, 10, 15, 5017, 1153, 2, EffectLayer.Waist); // Create a magical particle effect around the caster

                List<StatMod> mods = new List<StatMod>
                {
                    new StatMod(StatType.Str, "BattleFocusStr", 10, TimeSpan.FromSeconds(30.0)), // Increase Strength by 10
                    new StatMod(StatType.Dex, "BattleFocusDex", 10, TimeSpan.FromSeconds(30.0)), // Increase Dexterity by 10
                    new StatMod(StatType.Int, "BattleFocusInt", 10, TimeSpan.FromSeconds(30.0))  // Increase Intelligence by 10
                };

                foreach (StatMod mod in mods)
                {
                    Caster.AddStatMod(mod); // Apply each stat modifier
                }

                Caster.SendMessage("You feel a surge of power as your Battle Focus increases your abilities!");

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    foreach (StatMod mod in mods)
                    {
                        Caster.RemoveStatMod(mod.Name); // Remove the stat modifiers after the duration
                    }

                    Caster.PlaySound(0x1F8); // Play a sound indicating the end of the effect
                    Caster.FixedParticles(0x373A, 10, 15, 5017, 1153, 2, EffectLayer.Waist); // Create a particle effect indicating the end of the effect
                    Caster.SendMessage("The power of your Battle Focus fades away.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
