using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class HungersBane : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Hunger's Bane", "Eat and be satisfied!",
            //SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public HungersBane(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_FoodTypes = new Type[]
        {
            typeof(BreadLoaf),
            typeof(CheeseWedge),
            typeof(Apple),
            typeof(Peach),
            typeof(Grapes),
            typeof(MeatPie),
            typeof(CookedBird),
            typeof(Ham),
            typeof(Sausage)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    int itemCount = Utility.RandomMinMax(5, 10); // Summon between 5 and 10 food items

                    for (int i = 0; i < itemCount; i++)
                    {
                        Type foodType = m_FoodTypes[Utility.Random(m_FoodTypes.Length)];
                        Item foodItem = (Item)Activator.CreateInstance(foodType);

                        Point3D location = new Point3D(Caster.X + Utility.Random(-2, 5), Caster.Y + Utility.Random(-2, 5), Caster.Z);

                        foodItem.MoveToWorld(location, Caster.Map);

                        // Add a visual and sound effect at the food item location
                        Effects.SendLocationParticles(EffectItem.Create(location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(location, Caster.Map, 0x1F5);
                    }

                    // Display a flashy visual effect around the caster
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 9909);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x5C);
                }
                catch
                {
                    Caster.SendMessage("The spell failed to summon the food.");
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
