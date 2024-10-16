using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Grimly")]
    public class JesterGrimly : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterGrimly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Grimly";
            Body = 0x191; // Human female body for a jester appearance

            // Stats
            Str = 75;
            Dex = 80;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new JesterHat() { Hue = Utility.RandomBrightHue() });
            AddItem(new JesterSuit() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBrightHue() });
            AddItem(new BodySash() { Hue = Utility.RandomBrightHue() });

            // Speech Hue
            SpeechHue = Utility.RandomBrightHue();

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
                Say("Ah, greetings! I am Jester Grimly, the merry wanderer of jests and jokes. What brings you to my whimsical world?");
            }
            else if (speech.Contains("jester"))
            {
                Say("Yes, a jester I am! My job is to entertain and amuse. What else would you like to know?");
            }
            else if (speech.Contains("job"))
            {
                Say("Indeed, my job is to spread cheer and laughter. But there's more to it. Have you heard of my marvelous tricks?");
            }
            else if (speech.Contains("tricks"))
            {
                Say("Ah, tricks are the essence of my craft! They bring joy and surprise. Speaking of joy, have you ever been to a carnival?");
            }
            else if (speech.Contains("carnival"))
            {
                Say("The carnival! A place of merriment and wonder. But there's a secret I must share. Do you enjoy a good jest?");
            }
            else if (speech.Contains("jest"))
            {
                Say("A jest, you say? It’s a delightful jest that keeps spirits high. Tell me, do you know what mischief is?");
            }
            else if (speech.Contains("mischief"))
            {
                Say("Mischief is the spice of life! It adds excitement and unpredictability. What about riddles? Do you enjoy them?");
            }
            else if (speech.Contains("riddles"))
            {
                Say("Riddles, the ultimate test of wit! They challenge the mind and entertain the soul. Speaking of challenges, are you ready for a reward?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge, indeed! Only the clever and dedicated shall succeed. Have you ever heard of the Jester’s Jest?");
            }
            else if (speech.Contains("jest"))
            {
                Say("Yes, the Jester’s Jest! It's a treasure chest filled with delightful surprises. But only those who solve the puzzle can claim it.");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("A puzzle it is! Solve the riddle of joy and mischief, and you shall be rewarded. But first, tell me, do you have the key to unlock the chest?");
            }
            else if (speech.Contains("key"))
            {
                Say("The key to unlock the chest is the knowledge of jest and joy. If you have followed the clues and found the answer, you may be rewarded.");
            }
            else if (speech.Contains("answer"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The time for your reward has not yet arrived. Return later for your prize.");
                }
                else
                {
                    Say("Congratulations! You have solved the puzzle and unlocked the reward. Here is the Jester’s Jest, a treasure for the worthy.");
                    from.AddToBackpack(new JestersJest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I’m afraid I don’t understand that. Perhaps try asking about my name, job, or something related to jests and merriment?");
            }

            base.OnSpeech(e);
        }

        public JesterGrimly(Serial serial) : base(serial) { }

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
