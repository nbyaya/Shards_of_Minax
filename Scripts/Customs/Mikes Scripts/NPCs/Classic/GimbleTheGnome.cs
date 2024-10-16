using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gimble the Gnome")]
    public class GimbleTheGnome : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GimbleTheGnome() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gimble the Gnome";
            Body = 0x1C; // Gnome body
            Hue = 0xB;   // Skin color for gnomes

            // Stats
            Str = 80;
            Dex = 80;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new FloppyHat() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new GnarledStaff() { Name = "Gimble's Lute" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Gimble the Gnome!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in fine fettle, I am!");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a gnome of many talents, but my main job is tending to the ancient garden nearby.");
            }
            else if (speech.Contains("garden"))
            {
                Say("The garden is a place of beauty and serenity, a testament to the virtue of Humility. Would you like to learn more about it?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, the garden is a place of tranquility where one can contemplate the virtues. It teaches us that even in the smallest of places, great wisdom can be found. What virtue interests you the most?");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is the foundation of all virtues. It reminds us to be humble in our actions and thoughts, knowing that we are but a small part of the vast universe. Have you ever faced a moment that tested your humility?");
            }
            else if (speech.Contains("moment"))
            {
                Say("Ah, life is full of such tests. For me, tending to the garden has often been a lesson in humility. Plants, with their silent growth and resilience, teach us much. Have you ever tried gardening?");
            }
            else if (speech.Contains("gardening"))
            {
                Say("Gardening is not just about planting seeds and watching them grow. It's about patience, understanding, and nurturing. If you're ever interested, I can offer you some seeds to start your own garden. Interested?");
            }
            else if (speech.Contains("seeds"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Wonderful! Here, take these seeds. They are a special variety that I've cultivated over the years. May they bring joy and wisdom to your life. Remember, patience is key. Have you ever cultivated something with patience?");
                    from.AddToBackpack(new ShirtSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("patience"))
            {
                Say("Ah, patience is a true virtue. It's not just about waiting, but about how you wait. Being at peace during the process is as important as the outcome. What else piques your curiosity about virtues or life?");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the spark that ignites the flame of knowledge. It drives us to explore, learn, and grow. Always nurture your curiosity, traveler. For it can lead you to places you've never imagined. Speaking of which, have you explored the surrounding lands?");
            }
            else if (speech.Contains("lands"))
            {
                Say("The lands around here are filled with mysteries and wonders. Ancient ruins, enchanted forests, and hidden treasures await those who dare to venture. If you ever need guidance, come to me. I have traveled far and wide. Will you embark on such adventures?");
            }
            else if (speech.Contains("adventures"))
            {
                Say("Adventures test our mettle and shape our character. They challenge us in ways we can't foresee. I've had my share of adventures, and they've made me who I am today. If you're ever in need, remember to seek out the wisdom in the world. And as a token for our conversation, here's a small reward to aid you on your journey.");
                // Action for reward should be handled here
            }

            base.OnSpeech(e);
        }

        public GimbleTheGnome(Serial serial) : base(serial) { }

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
