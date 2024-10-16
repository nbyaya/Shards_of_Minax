using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Belle the Bubbly")]
    public class BelleTheBubbly : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BelleTheBubbly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Belle the Bubbly";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new JesterHat(1153)); // Jester hat with hue 1153
            AddItem(new JesterSuit(1150)); // Jester suit with hue 1150
            AddItem(new Shoes(1912)); // Shoes with hue 1912

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler! I am Belle the Bubbly, the jester of this realm!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm feeling quite bubbly today!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to make people laugh and spread joy!");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Laughter is the key to joy! What do you think?");
            }
            else if (speech.Contains("clever"))
            {
                Say("Ah, a clever response! Laughter and wit go hand in hand!");
            }
            else if (speech.Contains("belle"))
            {
                Say("Ah, you've heard of me? Not surprising! Many tales of my jests and tricks have spread far and wide!");
            }
            else if (speech.Contains("bubbly"))
            {
                Say("Bubbly isn't just my name, but also my nature! Life's too short not to be in high spirits. Have you ever felt bubbly?");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is not just a feeling but a mission for me. I've traveled many places to share it. Do you seek joy in your travels?");
            }
            else if (speech.Contains("key"))
            {
                Say("Indeed, laughter is the key to many doors. Not just joy, but also understanding, friendship, and healing. Have you used this key recently?");
            }
            else if (speech.Contains("tales"))
            {
                Say("Some say that one of my tales holds a secret to a hidden treasure. But is it just another jest of mine or a truth waiting to be discovered? Fancy a hint?");
            }
            else if (speech.Contains("spirits"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("High spirits are contagious! For being a good sport, here's a little token of appreciation from me.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("travels"))
            {
                Say("During my travels, I've met various entertainers, each with their unique style. From minstrels to fire breathers, the world is full of wonders. Have you met any other entertainers?");
            }
            else if (speech.Contains("doors"))
            {
                Say("Each door opened with the key of laughter reveals a new world, a new perspective. I believe that every locked door has its own story. Ever stumbled upon a locked door during your adventures?");
            }

            base.OnSpeech(e);
        }

        public BelleTheBubbly(Serial serial) : base(serial) { }

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
