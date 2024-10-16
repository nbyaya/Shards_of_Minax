using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sage Thandor")]
    public class SageThandor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SageThandor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sage Thandor";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 75;
            Int = 100;
            Hits = 100;

            // Appearance
            AddItem(new Robe() { Hue = 2118 });
            AddItem(new Sandals() { Hue = 1105 });
            AddItem(new WideBrimHat() { Hue = 2118 });
            AddItem(new LeatherGloves() { Hue = 2118 });

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
                Say("Greetings, traveler. I am Sage Thandor.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a sage, dedicated to the pursuit of knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("True wisdom lies in understanding the virtues that guide our lives. Do you seek knowledge of these virtues?");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Then let us discuss the virtues of Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Which virtue intrigues you the most?");
            }
            else if (speech.Contains("thandor"))
            {
                Say("Ah, you recognize my name. I have lived for many a year and have seen the rise and fall of empires. Have you heard of the ancient prophecy?");
            }
            else if (speech.Contains("good"))
            {
                Say("Good health is not just about the body but also the mind and soul. I often meditate to find balance. Have you tried meditation?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is vast and endless. While I specialize in the virtues, there are others who seek knowledge of the arcane arts. Are you interested in magic?");
            }
            else if (speech.Contains("virtues") && speech.Contains("shrine"))
            {
                Say("The virtues are the bedrock upon which society is built. By understanding them, we find our path in life. Each virtue has a corresponding shrine. Have you visited them?");
            }
            else if (speech.Contains("honesty"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Honesty is the foundation of all virtues. Without it, all else is meaningless. For your genuine interest, I offer you this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("prophecy"))
            {
                Say("The prophecy speaks of a time when the land will be consumed by darkness, and only a hero pure of heart can dispel it. Do you believe you are that hero?");
            }
            else if (speech.Contains("meditation"))
            {
                Say("Meditation allows one to find inner peace and clarity. I have a special incense that aids in meditation. Would you like some?");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the manipulation of the natural world using arcane energies. It is a powerful tool but must be used wisely. Have you heard of the forbidden spells?");
            }
            else if (speech.Contains("shrine"))
            {
                Say("Each shrine is dedicated to a specific virtue. By meditating at these shrines, one can gain deeper understanding. Do you know of the lost shrine?");
            }

            base.OnSpeech(e);
        }

        public SageThandor(Serial serial) : base(serial) { }

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
