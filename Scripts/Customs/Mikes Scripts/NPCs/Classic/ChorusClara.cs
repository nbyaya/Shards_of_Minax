using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Chorus Clara")]
    public class ChorusClara : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ChorusClara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Chorus Clara";
            Body = 0x191; // Human female body

            // Stats
            Str = 119;
            Dex = 67;
            Int = 82;
            Hits = 86;

            // Appearance
            AddItem(new FancyDress() { Hue = 94 }); // Fancy dress with hue 94
            AddItem(new Boots() { Hue = 48 }); // Boots with hue 48
            AddItem(new LeatherGloves() { Name = "Clara's Chorus Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Chorus Clara, the wandering bard!");
            }
            else if (speech.Contains("job"))
            {
                Say("I sing songs of valor and courage!");
            }
            else if (speech.Contains("tales") || speech.Contains("valor") || speech.Contains("courage"))
            {
                Say("Are you interested in tales of bravery?");
            }
            else if (speech.Contains("valor") || speech.Contains("virtue"))
            {
                Say("Valor is the essence of bravery, a virtue to be admired.");
            }
            else if (speech.Contains("path") || speech.Contains("adventures") || speech.Contains("valor"))
            {
                Say("Do you follow the path of valor in your adventures?");
            }
            else if (speech.Contains("hero") || speech.Contains("heart") || speech.Contains("valor"))
            {
                Say("Perhaps you have the heart of a true hero!");
            }
            else if (speech.Contains("chorus"))
            {
                Say("I've wandered many lands, singing tales from the frozen tundras to the scorching deserts.");
            }
            else if (speech.Contains("health"))
            {
                Say("Ah, my health? It's been better. The roads can be perilous and the nights cold, but my spirit remains strong.");
            }
            else if (speech.Contains("songs"))
            {
                Say("Songs have the power to inspire and uplift. There's a special one I composed recently about a lost city.");
            }
            else if (speech.Contains("tales"))
            {
                Say("One of my favorites is about the knight Sir Lancel, who faced a dragon to save a village.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("There are many virtues, but valor is among the most cherished. Many have sought to truly embody it.");
            }
            else if (speech.Contains("path"))
            {
                Say("Every path we take shapes our destiny. I once took a path through a haunted forest, and it changed me forever.");
            }
            else if (speech.Contains("hero"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If you truly possess the heart of a hero, I'd like to reward you with a special song I've composed. Would you like to hear it?");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward (replace SpecialSong with appropriate item)
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ChorusClara(Serial serial) : base(serial) { }

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
