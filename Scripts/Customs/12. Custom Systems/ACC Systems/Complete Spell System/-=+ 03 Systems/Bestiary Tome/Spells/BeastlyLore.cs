using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class BeastlyLore : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Beastly Lore", "In Ani Lor",
            21005,
            9400,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } } // Cast delay in seconds
        public override double RequiredSkill { get { return 50.0; } } // Minimum skill required to cast
        public override int RequiredMana { get { return 15; } } // Mana required to cast

        public BeastlyLore(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x64C); // Sound effect for casting

                // Visual effect at the caster's location
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1160, 0, 2023, 0);

                // Apply skill boost
                Caster.Skills[SkillName.Veterinary].Base += 20; // Temporarily increase Veterinary skill

                // Add a timer to remove the skill boost after a duration
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Skills[SkillName.Veterinary].Base -= 20; // Remove the skill boost
                        Caster.SendMessage("Your enhanced understanding of beastly lore fades away.");
                    }
                });

                FinishSequence();
            }
            else
            {
                Caster.SendLocalizedMessage(502632); // Failed to cast spell
            }
        }
    }
}
