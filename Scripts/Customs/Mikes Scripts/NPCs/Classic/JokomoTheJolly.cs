using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jokomo the Jolly")]
    public class JokomoTheJolly : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JokomoTheJolly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jokomo the Jolly";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 90;
            Hits = 90;

            // Appearance
            AddItem(new JesterHat() { Hue = 38 });
            AddItem(new JesterSuit() { Hue = 1324 });
            AddItem(new Boots() { Hue = 1102 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Ho ho! I am Jokomo the Jolly, the jester of riddles and merriment!");
            }
            else if (speech.Contains("health"))
            {
                Say("Ah, my health? The laughter keeps me hearty and hale!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? To spread joy and wisdom through riddles and jests!");
            }
            else if (speech.Contains("virtues") && speech.Contains("humility"))
            {
                Say("Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility - these virtues intertwine like the threads of fate.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("But dost thou know of the virtues, those eight guiding lights of Britannia?");
            }
            else if (speech.Contains("jokomo"))
            {
                Say("Oh, the tales behind my name are vast and varied! But the most cherished one speaks of a riddle that saved a kingdom. Would you like to hear it?");
            }
            else if (speech.Contains("laughter"))
            {
                Say("They say laughter is the best medicine, and I believe it's true! Every chuckle, every giggle, they all add life to my years. Do you enjoy a good jest?");
            }
            else if (speech.Contains("riddles"))
            {
                Say("Riddles are more than just words, they are a window into the soul! If you can solve my favorite riddle, I might just have a special reward for you.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Very well! Here's a riddle for you: \"I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.\" What am I?");
            }
            else if (speech.Contains("jest"))
            {
                Say("Jests and jokes are the sparks that light up our souls! Here's one for you: Why did the scarecrow win an award? Because he was outstanding in his field!");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Ah, you seek the reward! Solve my riddle correctly, and something valuable will be yours. But remember, the true reward lies in the journey of discovery!");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("echo"))
            {
                Say("Correct! Here is your reward!");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
            }

            base.OnSpeech(e);
        }

        public JokomoTheJolly(Serial serial) : base(serial) { }

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
