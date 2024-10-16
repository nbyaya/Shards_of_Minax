using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class NaturesAlly : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Nature’s Ally", "Foo Vi Lox",
            // SpellCircle.Fourth, // You can specify a circle if necessary
            21005,
            9400,
            false,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss,
            Reagent.Ginseng
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Adjust circle as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly Type[] m_AnimalTypes = new Type[]
        {
            typeof(TimberWolf),
            typeof(GreatHart),
            typeof(Eagle),
            typeof(BlackBear)
        };

        public NaturesAlly(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Select a random animal type to summon
                    Type animalType = m_AnimalTypes[Utility.Random(m_AnimalTypes.Length)];
                    BaseCreature summonedAnimal = (BaseCreature)Activator.CreateInstance(animalType);

                    // Define duration based on caster's skill level
                    TimeSpan duration = TimeSpan.FromSeconds(30.0 + (Caster.Skills[CastSkill].Value * 0.2));

                    // Summon the creature with effects
                    SpellHelper.Summon(summonedAnimal, Caster, 0x215, duration, false, false);

                    // Play sound and visual effects on summon
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x64);
                    Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    summonedAnimal.FixedParticles(0x3728, 10, 15, 5042, EffectLayer.Waist);

                    summonedAnimal.Say("*A loyal ally from nature appears!*");
                }
                catch (Exception ex)
                {
                    Caster.SendMessage("Something went wrong while summoning your ally.");
                    Console.WriteLine(ex.Message);
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
