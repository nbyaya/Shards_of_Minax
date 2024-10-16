using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class StalwartStance : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stalwart Stance", "Fortis Vires",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public StalwartStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You assume a stalwart stance, mimicking the stability of a fortress!");

                // Apply the defense and resistance buffs
                Caster.VirtualArmorMod += 20; // Increase armor by 20 points

                // Create and apply resistance modification
                ResistanceMod resistanceMod = new ResistanceMod(ResistanceType.Physical, 15);
                Caster.AddResistanceMod(resistanceMod);

                // Apply visual effects
                Caster.FixedParticles(0x376A, 10, 15, 5018, EffectLayer.Waist); // Sparkle effect around waist
                Caster.PlaySound(0x1E9); // Play a defense sound effect

                // Start a timer to revert the buffs after 30 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => EndEffect(Caster, resistanceMod));

                FinishSequence();
            }
        }

        private void EndEffect(Mobile caster, ResistanceMod resistanceMod)
        {
            caster.VirtualArmorMod -= 20; // Revert armor increase
            caster.RemoveResistanceMod(resistanceMod); // Remove resistance modification

            caster.SendMessage("Your stalwart stance fades away, and you feel your defenses return to normal.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
