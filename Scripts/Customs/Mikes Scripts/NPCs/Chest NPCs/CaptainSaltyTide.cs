using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Salty Tide")]
    public class CaptainSaltyTide : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainSaltyTide() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Salty Tide";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new TricorneHat() { Hue = 1157 });
            AddItem(new FancyShirt() { Hue = 1157 });
            AddItem(new ShortPants() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            AddItem(new Cutlass() { Hue = 1157 });

            // Custom hair and beard
            HairItemID = 0x203B; // Sea captain beard
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x203B; // Sea captain beard

            // Speech Hue
            SpeechHue = 1157; // Nautical color

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
                Say("Ahoy, matey! I'm Captain Salty Tide, the bravest sailor of the seven seas!");
                Say("If ye be seekin' adventure, I can tell ye tales of the ocean's mysteries.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Aye, adventure awaits those who dare to brave the storms! But tell me, what do ye know of the sea?");
            }
            else if (speech.Contains("sea"))
            {
                Say("The sea be full of wonders and perils. From stormy weather to hidden treasures, it's a vast expanse!");
                Say("Tell me, have ye ever heard of the mysterious sea creatures that dwell beneath?");
            }
            else if (speech.Contains("creatures"))
            {
                Say("Aye, the sea creatures be both wondrous and fearsome. Some say the Kraken still lurks in the depths!");
                Say("But enough about creatures, what say ye to a test of wit? Solve me riddle, and ye may earn a reward!");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Here's the riddle: What has a heart that doesn’t beat, but still keeps its rhythm? Solve that, and ye might earn a treasure!");
            }
            else if (speech.Contains("heart"))
            {
                Say("Ah, the heart! It can be a symbol of life or mystery. But remember, the rhythm is what matters here.");
            }
            else if (speech.Contains("rhythm"))
            {
                Say("Indeed, rhythm guides the tides and the heartbeat of the ocean. Still, the answer to me riddle eludes ye.");
            }
            else if (speech.Contains("answer"))
            {
                Say("Aye, the answer be 'a sea'. It has no heartbeat, but the rhythm of its waves is constant.");
                Say("If ye've figured that out, it shows ye have the makings of a true sailor. But there be more to discover!");
            }
            else if (speech.Contains("discover"))
            {
                Say("To discover is to explore, to find what others have missed. Seek ye knowledge of the ocean's treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The sea holds many treasures, but they be hidden and guarded. A chest, for instance, can be a great reward!");
                Say("If ye prove yer worth, I’ll grant ye the Sea Dog's Chest with its hidden treasures.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Prove yer worth by showing ye understand the mysteries of the sea and the tales of old. The Sea Dog's Chest awaits!");
            }
            else if (speech.Contains("chest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've no reward for ye right now. Come back later, and I'll have somethin' for ye.");
                }
                else
                {
                    Say("Arr! Ye've shown great understanding of the sea’s mysteries! For yer cleverness, accept this Sea Dog's Chest as a reward.");
                    from.AddToBackpack(new SeaDogsChest()); // Give the Sea Dog's Chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CaptainSaltyTide(Serial serial) : base(serial) { }

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
