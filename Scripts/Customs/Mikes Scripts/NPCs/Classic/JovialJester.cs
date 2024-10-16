using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of a jovial jester")]
    public class JovialJester : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JovialJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jovial Jester";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 100;
            Int = 100;
            Hits = 100;

            // Appearance
            AddItem(new FancyShirt() { Hue = 2118 });
            AddItem(new LongPants() { Hue = 2118 });
            AddItem(new JesterHat() { Hue = 2118 });
            AddItem(new LeatherGloves() { Hue = 2118 });
            AddItem(new Shoes() { Hue = 2118 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, traveler! I am the Jovial Jester.");
            }
            else if (speech.Contains("jokes"))
            {
                Say("I am the Jovial Jester, here to lighten thy spirit!");
            }
            else if (speech.Contains("joke"))
            {
                Say("Why did the chicken cross the road? To get to the other side, of course!");
            }
            else if (speech.Contains("riddles"))
            {
                Say("Do you enjoy riddles, my friend?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Why is a raven like a writing desk? A raven is like a writing desk because Poe wrote on both!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm always in good spirits, even if my feet hurt from all the dancing. It's just a jester's life, you know.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to make people laugh, to forget their worries even if just for a moment. But sometimes, between jests, I whisper secrets.");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is the essence of life! And in my travels, I've learned many things that bring joy to people. One of them is the power of the mantras. Especially the mantra of Humility.");
            }
            else if (speech.Contains("dancing"))
            {
                Say("Ah, dancing! It's a jesters' way of expressing happiness. If you ever want to learn some moves, let me know!");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, a seeker of truths! Very well, one secret I shall share: the third syllable of the mantra of Humility is MUH.");
                    from.AddToBackpack(new Gold(1000)); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("mantra"))
            {
                Say("Mantras are powerful words of magic, each carrying deep meaning. The mantra of Humility is especially potent.");
            }
            else if (speech.Contains("moves"))
            {
                Say("A little jig here, a little twirl there. A jester's moves are designed to capture attention and lighten the mood!");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is a virtue, my friend. It reminds us to be grounded and not be swayed by pride. Remember, the third syllable is MUH.");
            }
            else if (speech.Contains("mood"))
            {
                Say("Mood swings are common, but with a little jest and riddle, one can always find a reason to smile!");
            }
            else if (speech.Contains("jovial"))
            {
                Say("Ah, thou recognizest me! Indeed, I am the renowned Jovial Jester. Here to spread joy throughout the land.");
            }

            base.OnSpeech(e);
        }

        public JovialJester(Serial serial) : base(serial) { }

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
