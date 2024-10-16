using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dexter Byte")]
    public class DexterByte : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DexterByte() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dexter Byte";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new StuddedLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherCap() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Dexter's Guide to Gaming" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2045, 0x2049); // Various hair styles
            HairHue = Utility.RandomHairHue();

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
                Say("Hey there, I'm Dexter Byte, your gaming guru! Want to know more about my job?");
            }
            else if (speech.Contains("job"))
            {
                Say("I’m here to share gaming tips and hand out rewards to those who prove their worth. Interested in a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Ready for a challenge? Show your gaming spirit, and you might win a special prize!");
            }
            else if (speech.Contains("spirit"))
            {
                Say("Gaming spirit is all about passion and perseverance. What kind of games do you enjoy?");
            }
            else if (speech.Contains("games"))
            {
                Say("I’ve played all kinds of games, from ancient puzzles to modern adventures. Any particular type you excel at?");
            }
            else if (speech.Contains("excel"))
            {
                Say("Excellence in gaming comes with practice and knowledge. Have you mastered any specific game mechanics?");
            }
            else if (speech.Contains("mechanics"))
            {
                Say("Game mechanics are the rules that define gameplay. Mastering them can lead to great rewards. Do you seek knowledge of rare game treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are hidden throughout the gaming world. To find them, one must be clever and attentive. Are you ready for a special quest?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest requires dedication and skill. Show me your dedication, and you might earn a valuable reward.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication in gaming often leads to unexpected discoveries. Have you discovered any hidden secrets recently?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets can be powerful and mysterious. Speaking of mysteries, have you ever solved a difficult gaming puzzle?");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("Puzzles challenge the mind and are a key part of many games. Solve one of my gaming puzzles, and you could earn a reward.");
            }
            else if (speech.Contains("solve"))
            {
                Say("To solve a puzzle, one must think creatively and logically. Ready to tackle a puzzle challenge?");
            }
            else if (speech.Contains("creative"))
            {
                Say("Creativity in gaming often leads to innovative solutions. How do you approach creative problem-solving in games?");
            }
            else if (speech.Contains("problem-solving"))
            {
                Say("Effective problem-solving involves analyzing and adapting. Do you enjoy sharing your strategies with others?");
            }
            else if (speech.Contains("strategies"))
            {
                Say("Strategies are key to succeeding in games. Share your best strategy with me, and you might be rewarded.");
            }
            else if (speech.Contains("succeeding"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You've already received a reward recently. Come back later for more!");
                }
                else
                {
                    Say("Congratulations! Your dedication and creativity have earned you a special prize. Here’s the Gamer's Lootbox!");
                    from.AddToBackpack(new GamersLootbox()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I’m here to assist with gaming knowledge and challenges. Feel free to ask me about anything related to games!");
            }

            base.OnSpeech(e);
        }

        public DexterByte(Serial serial) : base(serial) { }

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
