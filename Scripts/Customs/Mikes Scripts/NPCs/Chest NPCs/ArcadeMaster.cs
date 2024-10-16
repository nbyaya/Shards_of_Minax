using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Arcade Master")]
    public class ArcadeMaster : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ArcadeMaster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Arcade Master";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 80;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Cloak() { Hue = 1157 });
            AddItem(new JesterHat() { Hue = 1153 });
            AddItem(new Spellbook() { Name = "High Score Record Book" });
            
            Hue = 0; // Skin color
            HairItemID = 8253; // Short Hair
            HairHue = 1109; // Dark Hair

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
                Say("Greetings, player! I am the Arcade Master.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in perfect health, ready to play!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to oversee the arcade and reward the best players.");
            }
            else if (speech.Contains("arcade"))
            {
                Say("The arcade is filled with many challenging games. Only the best can achieve the highest scores.");
            }
            else if (speech.Contains("games"))
            {
                Say("From classic mazes to epic battles, our games test both skill and strategy.");
            }
            else if (speech.Contains("high score"))
            {
                Say("Achieving the high score is the ultimate goal. It takes skill, patience, and a bit of luck.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You've already received a reward recently. Please come back later.");
                }
                else
                {
                    Say("You've proven yourself worthy of a special reward. Take this Arcade Master's Vault as a token of your skill.");
                    from.AddToBackpack(new ArcadeMastersVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("challenge"))
            {
                Say("Every game is a challenge. The harder the game, the greater the reward.");
            }
            else if (speech.Contains("skill"))
            {
                Say("Skill is honed through practice. Keep playing, and you'll get better with each attempt.");
            }
            else if (speech.Contains("luck"))
            {
                Say("A little bit of luck never hurts, but true champions rely on their skills.");
            }

            base.OnSpeech(e);
        }

        public ArcadeMaster(Serial serial) : base(serial) { }

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
