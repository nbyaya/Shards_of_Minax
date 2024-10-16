using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sorceress Lirea")]
    public class SorceressLirea : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SorceressLirea() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sorceress Lirea";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 70;
            Int = 90;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1166 });
            AddItem(new Cap() { Hue = 1157 });
            AddItem(new LeatherGloves() { Hue = 1157 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Speech Hue
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
                Say("Greetings, traveler. I am Sorceress Lirea.");
            }
            else if (speech.Contains("health"))
            {
                Say("I sense the flow of magic within you.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a guardian of ancient knowledge and secrets.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Magic and virtue are intertwined like the threads of fate. Do you seek knowledge of the virtues?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then let us ponder the virtues together. What virtue resonates most with your heart?");
            }
            else if (speech.Contains("sorceress"))
            {
                Say("I've traveled across the lands, mastering the arts of magic. From the highest peaks to the lowest valleys, my name is both respected and feared.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the essence of life. It courses through our veins and connects us with the world. Though I am physically well, it is this connection that truly sustains me.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I guard are not just of spells and potions. They are of histories forgotten, and tales of old that shaped our world. Seek them, and you might find greater purpose.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues are the bedrock of our morality. They guide our actions and mold our souls. There are many, like Compassion, Honesty, and Valor. Each one is a path to enlightenment.");
            }
            else if (speech.Contains("traveled"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In my journeys, I've met beings of light and darkness. Each encounter was a lesson, teaching me about the balance of the universe. For your perseverance in seeking knowledge, accept this token from a distant land.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of magic is a force intertwined with nature. By harnessing this essence, we can manipulate the elements, heal wounds, and even glimpse the future.");
            }
            else if (speech.Contains("histories"))
            {
                Say("Long ago, our land was united under a single banner. But time and ambition fractured this unity, leading to an era of strife. It is crucial to remember our past, for it shapes our present.");
            }

            base.OnSpeech(e);
        }

        public SorceressLirea(Serial serial) : base(serial) { }

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
