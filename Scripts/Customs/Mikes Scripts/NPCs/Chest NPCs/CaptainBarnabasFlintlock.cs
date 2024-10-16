using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Barnabas Flintlock")]
    public class CaptainBarnabasFlintlock : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainBarnabasFlintlock() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Barnabas Flintlock";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Buccaneer";

            // Pirate attire
            AddItem(new TricorneHat(Utility.RandomNeutralHue()));
            AddItem(new Doublet(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Bandana(Utility.RandomNeutralHue()));

            // Stats
            Str = 100;
            Dex = 75;
            Int = 60;
            Hits = 85;

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
                Say("Ahoy! I be Captain Barnabas Flintlock, the fiercest buccaneer ye ever laid eyes on!");
            }
            else if (speech.Contains("health"))
            {
                Say("Arr, I be in fine health for a man o' the sea! A bit rough around the edges, but still hearty.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Why, I be the keeper of me own treasure chest, filled with loot and plunder from me many adventures!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ye be lookin' for treasure, eh? Well, ye best be prepared to prove yer worth, or ye’ll be walkin' the plank!");
            }
            else if (speech.Contains("worth"))
            {
                Say("Aye, if ye be worthy, ye shall receive a grand reward. But first, answer me this: What be the true mark of a pirate?");
            }
            else if (speech.Contains("pirate"))
            {
                Say("The true mark of a pirate is his spirit of adventure and cunning! Prove ye have these, and ye’ll earn yer reward.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of adventure drives us to seek the unknown and conquer the impossible. It’s what makes a true pirate!");
            }
            else if (speech.Contains("adventure"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Arr, I’ve no reward to give at the moment. Return when the winds be more favorable.");
                }
                else
                {
                    Say("Ye’ve proven yerself a true adventurer! Take this Buccaneer’s Chest, filled with treasures and trinkets from the seven seas.");
                    from.AddToBackpack(new BuccaneersChest()); // Give the BuccaneersChest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public CaptainBarnabasFlintlock(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
