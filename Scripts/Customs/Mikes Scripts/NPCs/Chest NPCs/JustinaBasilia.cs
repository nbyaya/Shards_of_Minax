using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Justina Basilia")]
    public class JustinaBasilia : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JustinaBasilia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Justina Basilia";
            Title = "the Byzantine Historian";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals(Utility.RandomBlueHue()));
            AddItem(new PlateHelm(Utility.RandomMetalHue()));

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
                Say("Greetings, traveler. I am Justina Basilia, the Byzantine Historian.");
            }
            else if (speech.Contains("job"))
            {
                Say("I preserve and recount the ancient tales of the Byzantine Empire.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for your concern.");
            }
            else if (speech.Contains("justina"))
            {
                Say("Ah, you have heard of me! I was once a prominent historian in the court of Emperor Justinian.");
            }
            else if (speech.Contains("byzantine"))
            {
                Say("The Byzantine Empire was a realm of great intrigue and splendor. Many treasures lie hidden within its history.");
            }
            else if (speech.Contains("history"))
            {
                Say("History is a tapestry of triumphs and tragedies. To understand the present, one must study the past.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasures of the Byzantine era are said to be imbued with magical properties. Seek them out with a discerning eye.");
            }
            else if (speech.Contains("magical"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Please return later.");
                }
                else
                {
                    Say("Your quest for knowledge and treasure has earned you a reward. Please accept this 'Emperor Justinian's Cache'.");
                    from.AddToBackpack(new EmperorJustinianCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public JustinaBasilia(Serial serial) : base(serial) { }

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
