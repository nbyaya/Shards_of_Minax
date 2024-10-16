using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class BeastWhisperer : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Beast Whisperer", "Vocis Bestiarum",
                                                        21005, // Gump ID
                                                        9400   // Spell icon
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        private static readonly int[] Sounds = { 0x64, 0x65, 0x66 }; // Random animal sounds

        public BeastWhisperer(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Temporary skill boost duration
                TimeSpan duration = TimeSpan.FromSeconds(30.0);

                // Apply skill boosts
                AddSkillMod(Caster, SkillName.AnimalLore, 10);
                AddSkillMod(Caster, SkillName.Tracking, 20);

                // Add a timer to remove the skill boosts after the duration
                Timer.DelayCall(duration, () =>
                {
                    RemoveSkillMods(Caster);
                });

                // Visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, Sounds[Utility.Random(Sounds.Length)]);
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.SendMessage(0x35, "You feel in tune with the beasts around you!");

                // Display effect on nearby mobiles (to make it flashy)
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m.Player)
                    {
                        m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                        m.SendMessage(0x35, "You feel the whisper of the wild...");
                    }
                }
            }

            FinishSequence();
        }

        private void AddSkillMod(Mobile caster, SkillName skill, double value)
        {
            // Add a skill mod to the caster
            caster.AddSkillMod(new DefaultSkillMod(skill, true, value));
        }

        private void RemoveSkillMods(Mobile caster)
        {
            // Remove all skill mods for AnimalLore and Tracking
            caster.RemoveSkillMod(new DefaultSkillMod(SkillName.AnimalLore, true, 0));
            caster.RemoveSkillMod(new DefaultSkillMod(SkillName.Tracking, true, 0));
            caster.SendMessage(0x22, "The whisper of the wild fades away...");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
