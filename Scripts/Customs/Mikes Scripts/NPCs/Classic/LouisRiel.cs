using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Louis Riel")]
    public class LouisRiel : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LouisRiel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Louis Riel";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 85;
            Int = 95;
            Hits = 75;

            // Appearance
            AddItem(new LongPants() { Hue = 1105 });
            AddItem(new Doublet() { Hue = 1140 });
            AddItem(new Boots() { Hue = 1170 });
            AddItem(new WideBrimHat() { Hue = 1140 });

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
                Say("Oh, another curious traveler, I suppose. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Why do you care? I'm not your healer, am I?");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? What a laugh! I'm just another pawn in this wretched world's game.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor? Ha! There's no valor in a world full of deceit and betrayal. But enough about me, what about you?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Hmm, you're either a fool or an optimist. Tell me, do you think you'll survive this harsh world?");
            }
            else if (speech.Contains("louis"))
            {
                Say("My name? I'm Louis Riel. But a name is just a label in this world, isn't it? Have you heard of the legend of my past?");
            }
            else if (speech.Contains("care"))
            {
                Say("Well, if you must know, my health has seen better days. This world takes a toll on you. Ever experienced the curse of this land?");
            }
            else if (speech.Contains("laugh"))
            {
                Say("I was once a leader, fighting for what I believed in. Now, I'm a mere shadow, haunted by my past decisions. Do you believe in redemption?");
            }
            else if (speech.Contains("legend"))
            {
                Say("Ah, the legend. It speaks of battles fought, of valor and sacrifice. But legends can be twisted tales. Have you ever sought the truth in them?");
            }
            else if (speech.Contains("curse"))
            {
                Say("The curse? It's not just of ill health. It's the curse of memories, of dreams lost. But for your kindness in asking, take this reward. It might help you on your journey.");
                // Reward
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new MaxxiaScroll()); // Adjust the item type as needed
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("redemption"))
            {
                Say("Redemption is a tricky thing. Some say it's never too late, others believe some deeds can never be undone. What's your take on forgiveness?");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is a double-edged sword. Sometimes it sets you free, other times it chains you down. But seeking it is a noble quest. Do you always seek the path of truth?");
            }
            else if (speech.Contains("reward"))
            {
                // This response is handled in the "curse" section already
                Say("If you seek a reward, speak of the curse or ask me again later.");
            }

            base.OnSpeech(e);
        }

        public LouisRiel(Serial serial) : base(serial) { }

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
