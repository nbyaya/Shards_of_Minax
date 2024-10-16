using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd John")]
    public class ShepherdJohn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdJohn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd John";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 45;
            Hits = 75;

            // Appearance
            AddItem(new LongPants() { Hue = 1121 });
            AddItem(new FancyShirt() { Hue = 1122 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd John's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

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
                Say("Greetings, traveler. I am Shepherd John, a humble shepherd.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, for I have the blessings of nature.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? I am a shepherd, tending to my flock and the secrets of the land.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The path of a shepherd is one of solitude and contemplation. Can you appreciate the simple life?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, the secrets of the land are not for all to know. You possess a curious spirit, traveler.");
            }
            else if (speech.Contains("flock"))
            {
                Say("My flock, they are more than just animals. They are a connection to this land and its history.");
            }
            else if (speech.Contains("history"))
            {
                Say("This land is old, filled with tales of valor and treachery. There's a particular story about a hidden treasure I once stumbled upon.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure. It's not gold or jewels, but a relic with mystical powers. It's hidden in a place where the sun and moon meet.");
            }
            else if (speech.Contains("relic"))
            {
                Say("The relic is an ancient amulet said to protect its bearer from dark forces. I can't use it as it's not for shepherds like me. If you promise to use it wisely, it's yours.");
            }
            else if (speech.Contains("promise"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your word has value, traveler. Very well. Here is the amulet. May it guard you on your journey.");
                    from.AddToBackpack(new ColdResistAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature is a guide, a teacher. When in doubt, I look to the rhythms of nature for answers.");
            }
            else if (speech.Contains("guide"))
            {
                Say("Ah, you seek guidance. The stars have been my guide at night, and a peculiar constellation has recently caught my attention.");
            }
            else if (speech.Contains("constellation"))
            {
                Say("It's called the Shepherd's Crook, a sign that change is coming. Keep an eye to the sky, traveler, and perhaps it will guide you too.");
            }

            base.OnSpeech(e);
        }

        public ShepherdJohn(Serial serial) : base(serial) { }

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
