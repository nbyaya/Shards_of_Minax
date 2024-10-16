using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Elias Cartwright")]
    public class CaptainEliasCartwright : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainEliasCartwright() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Elias Cartwright";
            Title = "the Explorer";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Stats
            Str = 100;
            Dex = 70;
            Int = 85;
            Hits = 90;

            // Appearance
            AddItem(new TricorneHat() { Hue = Utility.RandomNeutralHue() });
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Cloak() { Hue = Utility.RandomNondyedHue() });

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
                Say("Ahoy there! I am Captain Elias Cartwright, explorer and adventurer.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to uncover new lands and seek hidden treasures. I have traversed many seas and lands in search of riches.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as hearty as a fresh breeze off the Atlantic!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasure, you say? Aye, I have hidden many a treasure, but the most precious of all lies within that chest.");
            }
            else if (speech.Contains("chest"))
            {
                Say("The Colonial Pioneer's Cache holds secrets of the old world. If you wish to claim it, prove yourself worthy by showing me your curiosity.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the spark that drives discovery. Show me your thirst for knowledge and I may just reward you.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Please return later.");
                }
                else
                {
                    Say("Ah, your curiosity is commendable. As promised, take this Colonial Pioneer's Cache as your reward for your inquisitiveness.");
                    from.AddToBackpack(new ColonialPioneersCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I have tales of exploration and treasure aplenty. What else do you wish to know?");
            }

            base.OnSpeech(e);
        }

        public CaptainEliasCartwright(Serial serial) : base(serial) { }

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
