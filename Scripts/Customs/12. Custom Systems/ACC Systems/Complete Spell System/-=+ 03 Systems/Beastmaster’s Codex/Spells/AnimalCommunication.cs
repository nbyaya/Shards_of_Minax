using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.SkillHandlers;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class AnimalCommunication : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Communication", "Talkio Animus",
            21004, // Animation
            9300   // Effect sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust as necessary
        }

        public override double CastDelay { get { return 0.2; } } // Cast delay in seconds
        public override double RequiredSkill { get { return 20.0; } } // Required skill level
        public override int RequiredMana { get { return 20; } } // Mana cost

        public AnimalCommunication(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel more in tune with the animals around you.");

                // Temporary skill boost
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), ApplySkillBonus);
                
                // Visual and sound effects
                Caster.PlaySound(0x5A1); // Play a sound effect
                Caster.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist); // Particle effects around the caster
                
                Timer.DelayCall(TimeSpan.FromSeconds(30), RemoveSkillBonus); // Remove the bonus after 30 seconds
            }

            FinishSequence();
        }

        private void ApplySkillBonus()
        {
            if (Caster == null || Caster.Deleted)
                return;

            Caster.Skills[SkillName.AnimalLore].Base += 20; // Increase Animal Lore by 20
        }

        private void RemoveSkillBonus()
        {
            if (Caster == null || Caster.Deleted)
                return;

            Caster.Skills[SkillName.AnimalLore].Base -= 20; // Revert the skill increase
            Caster.SendMessage("The effects of Animal Communication have worn off.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
