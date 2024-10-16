using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gruk the Orc")]
    public class GrukTheOrc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrukTheOrc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gruk the Orc";
            Body = 0x190; // Orc body
            Hue = 0x835; // Default orc hue

            // Stats
            Str = 100;
            Dex = 60;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Name = "Gruk's Chestplate" });
            AddItem(new PlateArms() { Name = "Gruk's Armguards" });
            AddItem(new PlateLegs() { Name = "Gruk's Leggings" });
            AddItem(new PlateHelm() { Name = "Gruk's Helmet" });
            AddItem(new PlateGloves() { Name = "Gruk's Gauntlets" });
            AddItem(new Drums() { Name = "Gruk's Drum" });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(false); // Assuming the orc does not have a specific hair style, set it as needed
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
                Say("I am Gruk the Orc, the mighty drummer!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, for I drum with great vigor!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to keep the rhythm alive, with every beat of my drum!");
            }
            else if (speech.Contains("music"))
            {
                Say("Music can touch the soul and reveal one's true spirit. Dost thou understand the power of music?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, the power of music transcends words. Let us drum together in harmony!");
            }
            else if (speech.Contains("gruk"))
            {
                Say("Gruk not just any orc, Gruk chief of the Drumming Clan!");
            }
            else if (speech.Contains("robust"))
            {
                Say("Ah, robust like the sturdy wood of my drum. Only the strongest tree can produce such sound!");
            }
            else if (speech.Contains("rhythm"))
            {
                Say("Ah, rhythm, it's the heartbeat of the orcish community. It keeps us united and strong!");
            }
            else if (speech.Contains("power"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The power of music heals and inspires. For understanding this, I shall give you a reward!");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item if it has a different name or ID
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("clan"))
            {
                Say("Drumming Clan, yes. We believe in unity and brotherhood. Our drums carry our message far and wide.");
            }
            else if (speech.Contains("tree"))
            {
                Say("Trees provide more than just wood. They're also sacred, holding the spirit of the forest.");
            }
            else if (speech.Contains("community"))
            {
                Say("Our community thrives because we share a common pulse, a rhythm that echoes in all our hearts.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is strength. When we drum together, even the heavens can hear us!");
            }

            base.OnSpeech(e);
        }

        public GrukTheOrc(Serial serial) : base(serial) { }

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
