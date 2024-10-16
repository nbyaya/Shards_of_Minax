using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sassy Selena")]
    public class SassySelena : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SassySelena() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sassy Selena";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 75;
            Int = 55;
            Hits = 65;

            // Appearance
            AddItem(new LeatherArms() { Hue = 2956 });
            AddItem(new Kilt() { Hue = 2957 });
            AddItem(new ThighBoots() { Hue = 2960 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, sweet traveler. I am Sassy Selena, a courtesan of the night.");
            }
            else if (speech.Contains("health"))
            {
                Say("My beauty is my greatest asset, dear. I am in perfect health.");
            }
            else if (speech.Contains("job"))
            {
                Say("I entertain and delight those who seek companionship.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Compassion and allure are my greatest virtues. What virtues dost thou hold dear?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("That is a wise choice, my sweet. Compassion and allure can win over even the coldest hearts.");
            }
            else if (speech.Contains("selena"))
            {
                Say("Some call me enchanting, others bewitching. But you, my dear, may just call me Selena.");
            }
            else if (speech.Contains("beauty"))
            {
                Say("Beauty is not just about good health, but also about the secrets you keep to maintain it. Care to learn some of mine?");
            }
            else if (speech.Contains("entertain"))
            {
                Say("My work requires me to hear many tales. Some are whispers of forbidden love, others, tales of great adventures. Would you like to share your tale with me?");
            }
            else if (speech.Contains("virtues") && speech.Contains("valor"))
            {
                Say("There are many virtues in this world. Valor, Justice, and Honor to name a few. Which do you value the most?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the courage to face adversity. I've seen many brave souls in my time. But true valor comes from the heart. How have you demonstrated your valor?");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Ah, the path not taken. I too have faced moments of doubt. But I always find my way back. Sometimes, a simple song guides me. Would you like to hear it?");
            }
            else if (speech.Contains("song"))
            {
                Say("*singing* In the moon's soft glow, through the night we go. Hearts entwined, fate aligned, our souls forever to show. *singing*🎵");
            }
            else if (speech.Contains("moonflowers"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here you go, dear traveler. This essence will help rejuvenate your spirit. Use it wisely.");
                    from.AddToBackpack(new BeggingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tales"))
            {
                Say("Every tale holds a lesson, every adventure a memory. I have a keepsake from one such tale. Would you like to see?");
            }
            else if (speech.Contains("keepsake"))
            {
                Say("This is a locket, a memory of a past love. It reminds me of the fleeting nature of time. Life is but a series of moments. Cherish them.");
            }
            else if (speech.Contains("wandering"))
            {
                Say("Wanderers come and go, leaving traces of their stories. I keep a journal of such tales. Would you like to read an entry?");
            }

            base.OnSpeech(e);
        }

        public SassySelena(Serial serial) : base(serial) { }

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
