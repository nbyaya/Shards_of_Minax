using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Fizzy Fizzlepop")]
    public class FizzyFizzlepop : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FizzyFizzlepop() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Fizzy Fizzlepop";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyDress() { Hue = 1150 }); // Colorful and festive
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new JesterHat() { Hue = 1150 });

            Hue = Race.RandomSkinHue(); // Skin color
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
            // Speech Hue
            SpeechHue = 1150; // Pink for the whimsical theme

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            // Dialogue Tree
            if (speech.Contains("name"))
            {
                Say("Hello! I am Fizzy Fizzlepop, the Candy Conjurer!");
            }
            else if (speech.Contains("health"))
            {
                Say("I’m as bubbly and sweet as a fresh batch of candy!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to sprinkle a little magic and candy joy into the world!");
            }
            else if (speech.Contains("magic"))
            {
                Say("Ah, magic! It’s in every swirl of candy and every sprinkle of fairy dust.");
            }
            else if (speech.Contains("candy"))
            {
                Say("Candy is the essence of joy and magic. Have you ever tasted a candy that made you feel like you were floating on clouds?");
            }
            else if (speech.Contains("floating"))
            {
                Say("Floating on clouds sounds like a dream, doesn’t it? Perhaps there’s a bit of magic in every candy.");
            }
            else if (speech.Contains("dream"))
            {
                Say("Dreams are the gateways to magic. Sometimes, the sweetest dreams are made of candy and fairy tales.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Fairy tales are full of enchantment and wonder. Each one tells a story of magic, adventure, and sweets.");
            }
            else if (speech.Contains("wonder"))
            {
                Say("Wonder is the spark of magic in the world. Keep your heart open to it, and you’ll find surprises around every corner.");
            }
            else if (speech.Contains("surprises"))
            {
                Say("The best surprises are those that bring joy and a sprinkle of magic. Keep exploring, and you’ll discover many more!");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is the essence of life and magic. It’s what makes each piece of candy taste so special.");
            }
            else if (speech.Contains("special"))
            {
                Say("Special moments are often wrapped in magic and sweetness. Speaking of special, have you heard about the secret chest?");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the secret chest! It’s filled with the sweetest treasures and magical surprises. But you must prove your curiosity first.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the key to discovering magic and wonders. If you wish to see the chest, show me how curious you are about magic and candy.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I’ve already given out my magical rewards for now. Come back later for more sugary surprises!");
                }
                else
                {
                    Say("For your delightful curiosity and patience, here’s a special chest filled with the sweetest treasures!");
                    from.AddToBackpack(new SugarplumFairyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("sweet"))
            {
                Say("Sweetness is the essence of life and magic. Every piece of candy tells a story of joy and wonder.");
            }
            else if (speech.Contains("fairy"))
            {
                Say("Fairies are the keepers of magic and enchantment. They sprinkle a bit of magic wherever they go, just like candy.");
            }
            else if (speech.Contains("enchantment"))
            {
                Say("Enchantment is woven into every magical experience. It’s like the feeling you get when you taste the sweetest candy.");
            }
            else if (speech.Contains("feeling"))
            {
                Say("The feeling of wonder and joy is what makes life magical. It’s what makes each candy special and each moment memorable.");
            }
            else if (speech.Contains("moments"))
            {
                Say("Moments of joy and magic are to be cherished. They are what make life truly delightful and full of surprises.");
            }

            base.OnSpeech(e);
        }

        public FizzyFizzlepop(Serial serial) : base(serial) { }

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
