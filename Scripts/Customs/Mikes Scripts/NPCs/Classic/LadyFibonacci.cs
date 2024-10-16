using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Fibonacci")]
    public class LadyFibonacci : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyFibonacci() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Fibonacci";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 110;
            Hits = 65;

            // Appearance
            AddItem(new PlainDress() { Hue = 1130 }); // Clothing item with hue 1130
            AddItem(new Sandals() { Hue = 1130 }); // Sandals with hue 1130
            AddItem(new Spellbook() { Name = "Fibonacci's Sequence" });

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
                Say("Greetings, traveler. I am Lady Fibonacci, a philosopher of the arcane mysteries.");
            }
            else if (speech.Contains("health"))
            {
                Say("My physical state is of little concern. It is the state of the universe that intrigues me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation, you ask? I dwell in the realms of knowledge, exploring the boundaries of existence.");
            }
            else if (speech.Contains("philosophy") || speech.Contains("universe") || speech.Contains("knowledge"))
            {
                Say("The cosmos is a vast puzzle, and I seek the hidden patterns within. Do you yearn for knowledge, traveler?");
            }
            else if (speech.Contains("yes") || speech.Contains("potential") || speech.Contains("secrets"))
            {
                Say("Your response intrigues me. Perhaps you have the potential to unlock the secrets of the cosmos.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane arts are a window to the soul of the universe. They hold secrets that few dare to explore. Have you encountered the arcane in your journeys?");
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is not just what you see in the night sky. It's a web of energies, forces, and mysteries. Do you ever ponder about its infinite expanse?");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence is not just about living and dying. It's about understanding the very fabric of reality. Have you sought the deeper truths of our world?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key that unlocks the door to enlightenment. But acquiring it demands dedication and sacrifice. Are you willing to pay the price?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, secrets! They are the hidden treasures of the universe. I can share one with you, but first, prove your worth by answering this riddle: \"I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.\" What am I?");
            }
            else if (speech.Contains("wind"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Impressive! You've answered correctly. As promised, here is a reward for your keen intellect. Use it wisely.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyFibonacci(Serial serial) : base(serial) { }

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
