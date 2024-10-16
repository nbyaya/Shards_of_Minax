using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class AnimalCall : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Call", "Ani Cal", 
            266, // Icon
            9040 // Sound effect when casting
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        private static TimeSpan Duration = TimeSpan.FromMinutes(1);
        private static TimeSpan Cooldown = TimeSpan.FromMinutes(15);

        public AnimalCall(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_AnimalTypes = new Type[]
        {
            typeof(SummonedRandomCanine),
            typeof(SummonedRandomBear),
            typeof(SummonedRandomCat),
            typeof(SummonedRandomPig)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Type animalType = m_AnimalTypes[Utility.Random(m_AnimalTypes.Length)];

                try
                {
                    BaseCreature animal = (BaseCreature)Activator.CreateInstance(animalType);

                    // Play casting effects
                    Caster.PlaySound(0x214);
                    Caster.FixedParticles(0x3728, 1, 13, 9913, 92, 3, EffectLayer.Waist);

                    // Summon the animal
                    SpellHelper.Summon(animal, Caster, 0x215, Duration, false, false);

                    animal.Controlled = true;
                    animal.ControlMaster = Caster;
                    animal.IsBonded = true;

                    Caster.SendMessage("You have called a friendly animal companion!");

                    // Additional flashy effects
                    animal.FixedParticles(0x375A, 1, 30, 9964, 1153, 3, EffectLayer.Waist);
                    animal.PlaySound(0x1B5);
                }
                catch (Exception ex)
                {
                    Caster.SendMessage("Something went wrong while summoning the animal.");
                    Console.WriteLine(ex.ToString());
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }

    }  // Similarly, define FriendlyBear, FriendlyEagle, FriendlyPanther classes here.
}
