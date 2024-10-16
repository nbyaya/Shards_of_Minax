using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class EdibleIdentification : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Edible Identification", "Delicium Summus",
            21012, // Icon ID for the spell
            9308 // Cast sound effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 8; } }

        public EdibleIdentification(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play casting effect and sound
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head);
                Caster.PlaySound(0x213);

                // Generate random food item
                Item foodItem = GetRandomFoodItem();
                
                if (foodItem != null)
                {
                    // Add food item to caster's backpack
                    if (Caster.Backpack != null && Caster.Backpack.TryDropItem(Caster, foodItem, false))
                    {
                        // Success: Visual effects and sound
                        Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                        Caster.PlaySound(0x3E9); // Sound effect for item drop
                        Caster.SendMessage("A delicious treat has been identified and placed in your backpack!");
                    }
                    else
                    {
                        // Fail: Backpack is full or item drop failed
                        Caster.SendMessage("Your backpack is too full to receive the item.");
                        foodItem.Delete();
                    }
                }
                else
                {
                    // No item was created due to some error
                    Caster.SendMessage("The spell failed to summon any food item.");
                }
            }

            FinishSequence();
        }

        private Item GetRandomFoodItem()
        {
            // List of possible food items
            Type[] foodTypes = new Type[]
            {
                typeof(Apple),
                typeof(BreadLoaf),
                typeof(CheeseWheel),
                typeof(ChickenLeg),
                typeof(CookedBird),
                typeof(FishSteak),
                typeof(FruitPie),
                typeof(MeatPie),
                typeof(Peach),
                typeof(Pear)
            };

            // Randomly select a food type and instantiate it
            try
            {
                Type selectedFoodType = foodTypes[Utility.Random(foodTypes.Length)];
                return (Item)Activator.CreateInstance(selectedFoodType);
            }
            catch
            {
                return null; // If something goes wrong, return null
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
