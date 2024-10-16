using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class Featherweight : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Featherweight", "Alacris Arcus",
                                                        //SpellCircle.First,
                                                        21005,
                                                        9400
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public Featherweight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x4E); // Play a swift sound to signify activation
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x373A, 16, 10, 1153, 0); // Display a particle effect at the caster's location

                // Implementing the effect of reducing arrow weight and increasing carrying capacity
                Caster.SendMessage("Your arrows feel lighter, allowing you to carry more and move more swiftly!");

                if (Caster.Backpack != null)
                {
                    int count = 0;
                    foreach (Item item in Caster.Backpack.Items)
                    {
                        if (item is Arrow)
                        {
                            item.Weight *= 0.5; // Reduce weight of arrows by half
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        Caster.Dex += 10; // Temporary boost to Dexterity to signify increased swiftness
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            Caster.Dex -= 10; // Revert Dexterity boost after 30 seconds
                            foreach (Item item in Caster.Backpack.Items)
                            {
                                if (item is Arrow)
                                {
                                    item.Weight *= 2; // Revert arrow weight to original
                                }
                            }
                            Caster.SendMessage("The featherweight effect fades, and your arrows feel their normal weight.");
                        });
                    }
                    else
                    {
                        Caster.SendMessage("You have no arrows in your backpack to lighten.");
                    }
                }
                else
                {
                    Caster.SendMessage("You need a backpack to carry arrows.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
