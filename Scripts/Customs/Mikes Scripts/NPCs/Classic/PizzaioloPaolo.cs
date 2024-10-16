using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pizzaiolo Paolo")]
    public class PizzaioloPaolo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PizzaioloPaolo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pizzaiolo Paolo";
            Body = 0x190; // Human male body

            // Stats
            Str = 88;
            Dex = 50;
            Int = 62;
            Hits = 68;

            // Appearance
            AddItem(new ShortPants() { Hue = 38 });
            AddItem(new FancyShirt() { Hue = 295 });
            AddItem(new Shoes() { Hue = 38 });
            AddItem(new LeatherGloves() { Name = "Paolo's Pizza Mitts" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Welcome, traveler! I am Pizzaiolo Paolo, the master of pizzas!");
            }
            else if (speech.Contains("job"))
            {
                Say("Feast your eyes upon my culinary creations! They are a feast for the senses!");
            }
            else if (speech.Contains("health"))
            {
                Say("Every pizza I make is a work of art. It takes patience, skill, and compassion for flavors.");
            }
            else if (speech.Contains("battles"))
            {
                Say("But I must admit, I have my culinary battles. To create the perfect pizza, one must balance flavors.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Have you ever faced a culinary challenge, my friend? How did you overcome it?");
            }
            else if (speech.Contains("creations"))
            {
                Say("Ah, my creations! Each pizza tells a story of the lands and ingredients from which it is made. Would you like to hear about my most treasured recipe?");
            }
            else if (speech.Contains("recipe"))
            {
                Say("My most treasured recipe is the 'Lunar Tomato Delight'. The main ingredient is a rare tomato that only grows under the full moon. However, I've run out of them. Would you be able to help?");
            }
            else if (speech.Contains("help"))
            {
                Say("Splendid! If you bring me the Lunar Tomato, I will reward you for your effort. They are said to grow in the Moonlit Gardens, a mystical place not far from here.");
            }
            else if (speech.Contains("gardens"))
            {
                Say("The Moonlit Gardens are a magical place where plants grow under the influence of moonlight. But beware, not all that glitters in the garden is friendly. Tread carefully.");
            }
            else if (speech.Contains("tomato"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah! You've brought me the Lunar Tomato! As promised, here's a reward for your efforts.");
                    from.AddToBackpack(new WeaponSpeedAugmentCrystal()); // Replace with appropriate reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("flavors"))
            {
                Say("Flavors are the soul of a pizza. From the spicy tang of a pepperoni to the rich creaminess of mozzarella, each ingredient adds its voice to the symphony.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Ah, patience! It's not just waiting, but ensuring that each step, from kneading the dough to choosing the toppings, is done with love and care.");
            }
            else if (speech.Contains("skill"))
            {
                Say("Skill is acquired over years of practice. Every mistake I've made has taught me something new. From a burnt crust to an overload of cheese, I've seen it all.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("To truly understand the ingredients, one must have compassion for them. Each has its own journey, from the farm to my kitchen, and I respect that.");
            }
            else if (speech.Contains("no"))
            {
                Say("That's alright. Culinary challenges aren't for everyone. But if you ever decide to take one on, remember to approach it with an open heart and a hungry stomach!");
            }

            base.OnSpeech(e);
        }

        public PizzaioloPaolo(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
