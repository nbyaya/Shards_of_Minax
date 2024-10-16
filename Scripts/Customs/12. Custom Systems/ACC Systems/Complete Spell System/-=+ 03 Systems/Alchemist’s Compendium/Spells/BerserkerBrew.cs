using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class BerserkerBrew : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Berserker Brew", "Ferox Stimulare",
                                                        //SpellCircle.Fourth, // Not using a spell circle here
                                                        21005, // Effect ID
                                                        9301  // Animation ID
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public BerserkerBrew(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of power as you drink the Berserker Brew!");

                // Play a sound and visual effect on the caster
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F5); // Play a ferocious sound
                Caster.FixedParticles(0x373A, 1, 30, 9964, 4, 3, EffectLayer.Waist); // Red flame effect around caster

                // Apply the effects of the brew
                Caster.SendMessage("Your attack speed and damage are increased, but your defense is reduced!");

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.ReactiveArmor, 1044087, 1070722, TimeSpan.FromSeconds(30), Caster)); // Buff icon display

                Caster.AddStatMod(new StatMod(StatType.Dex, "BerserkerBrewDex", 10, TimeSpan.FromSeconds(30))); // Increase Dexterity for attack speed
                Caster.AddStatMod(new StatMod(StatType.Str, "BerserkerBrewStr", 10, TimeSpan.FromSeconds(30))); // Increase Strength for damage
                Caster.AddStatMod(new StatMod(StatType.Dex, "BerserkerBrewDef", -10, TimeSpan.FromSeconds(30))); // Reduce Dexterity for defense

                // Timer to remove the effect after 30 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveEffect());
            }

            FinishSequence();
        }

        private void RemoveEffect()
        {
            Caster.SendMessage("The effects of the Berserker Brew wear off, and you feel normal again.");

            Caster.RemoveStatMod("BerserkerBrewDex");
            Caster.RemoveStatMod("BerserkerBrewStr");
            Caster.RemoveStatMod("BerserkerBrewDef");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
