using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Luscious Lila")]
    public class LusciousLila : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LusciousLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Luscious Lila";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 50;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 2951 });
            AddItem(new Tunic() { Hue = 2950 });
            AddItem(new Sandals() { Hue = 2965 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Welcome, darling. I am Luscious Lila.");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, my health? Just dandy, my sweet. And yours?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Well, I provide... *services*, you see.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Oh, the battles I've seen in this... world. Care to share your stories, darling?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! You seem like you've got some stories to tell, my dear. So, have you ever faced a real challenge?");
            }
            else if (speech.Contains("services"))
            {
                Say("Services? Oh, darling, let's just say I have a particular set of skills that people pay for. And not the kind you'd expect! Interested?");
            }
            else if (speech.Contains("skills"))
            {
                Say("Ah, curious, are we? I can read palms, dance like a dream, and brew the most curious of potions. Ever tried a love potion, darling?");
            }
            else if (speech.Contains("potion"))
            {
                Say("A love potion is a delicate brew that can spark affection in even the coldest of hearts. But be warned, they come with a price. Would you like to purchase one?");
            }
            else if (speech.Contains("purchase"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, adventurous! For you, darling, it's a mere 50 gold coins. But as a token of my appreciation for our little chat, I'll give you this other mysterious potion for free. Drink it when you're ready for a surprise.");
                    from.AddToBackpack(new AlchemyAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("world"))
            {
                Say("Ah, this world is a tapestry of tales. Some of joy, some of sorrow. Do you have any favorite places you've been?");
            }
            else if (speech.Contains("places"))
            {
                Say("There are so many places with stories hidden in their shadows. The Whispering Woods, the Haunted Caves, and even the Serene Lake. Have you ever visited the Whispering Woods?");
            }
            else if (speech.Contains("woods"))
            {
                Say("Ah, the Whispering Woods. A place where trees speak if you listen closely. But it's not for the faint of heart. Dark creatures roam there. Stay safe, darling.");
            }
            else if (speech.Contains("creatures"))
            {
                Say("Yes, creatures like shadows that move without making a sound and eerie spirits that moan in the night. But if you're brave, treasures await the adventurous. Ever gone treasure hunting?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasures are more than just gold and gems, my dear. They are stories, memories, experiences. But if it's gold you seek, I've heard of a hidden chest in the woods. Just watch out for the creatures!");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the chest. It's said to be guarded by an ancient spirit. But if you can brave its challenges, the riches within are beyond imagination. Good luck, darling.");
            }

            base.OnSpeech(e);
        }

        public LusciousLila(Serial serial) : base(serial) { }

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
