using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Arcade King")]
    public class ArcadeKing : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ArcadeKing() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Arcade King";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 75;
            Int = 85;
            Hits = 80;

            // Appearance
            AddItem(new ChainChest() { Hue = 1159 });
            AddItem(new LongPants() { Hue = 1193 });
            AddItem(new LeatherArms() { Hue = 1953 });
            AddItem(new Boots() { Hue = 29153 });
            AddItem(new TricorneHat() { Hue = 1753 });

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
                Say("Ah, you seek my name? I am known as the Arcade King.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in great health, thank you for asking. The arcade games keep me young!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to oversee the arcade and ensure that everyone is having fun and playing fair.");
            }
            else if (speech.Contains("arcade"))
            {
                Say("The arcade is a place of joy and competition. I have many games and treasures for those who prove their worth.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasures, you say? Only those who can solve my riddles will earn a special reward.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("To earn the treasure, you must solve the following riddle: What has keys but can't open locks?");
            }
            else if (speech.Contains("piano"))
            {
                Say("Well done! You have solved the riddle. As a reward for your cleverness, please accept this special chest.");
                TimeSpan cooldown = TimeSpan.FromMinutes(15);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have already given you a reward recently. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new ArcadeKingsTreasure()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am not sure how to respond to that. Try asking me about my name, job, or health.");
            }

            base.OnSpeech(e);
        }

        public ArcadeKing(Serial serial) : base(serial) { }

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
