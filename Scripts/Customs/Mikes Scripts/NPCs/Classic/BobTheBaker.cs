using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bob the Baker")]
    public class BobTheBaker : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BobTheBaker() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bob the Baker";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 40;
            Int = 60;
            Hits = 50;

            // Appearance
            AddItem(new Skirt(1153));
            AddItem(new FancyShirt(1153));
            AddItem(new Boots(1153));
            AddItem(new Cap { Name = "Bob's Chef Hat" });
            AddItem(new Cleaver { Name = "Bob's Baking Knife" });

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
                Say("Greetings, traveler! I am Bob the Baker. How can I assist thee?");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in fine health, thank thee for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a humble baker, crafting delicious bread and pastries for the town. It's my life's work!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor, ah, it's a virtue of great importance! How dost thou define valor in thy life?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, true valor is indeed the strength of one's heart and the courage to face adversity with honor.");
            }
            else if (speech.Contains("pastries"))
            {
                Say("Ah, my pastries! They are my pride and joy. I make a special apple tart that's favored by many. Would you like to know the secret ingredient?");
            }
            else if (speech.Contains("ingredient"))
            {
                Say("Ah, a curious soul! My secret ingredient is a rare apple variety from the Whispering Orchard. They're not easy to find, but their flavor is unmatched. If you ever bring me some, I might reward you.");
            }
            else if (speech.Contains("orchard"))
            {
                Say("The Whispering Orchard lies to the east of this town. It's said the trees there whisper secrets to those who listen closely. But beware, some say the orchard is guarded by spirits.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Legend has it that the spirits of ancient guardians protect the orchard, ensuring only the worthy pick its fruits. But I believe they're just tales. Do you believe in such stories?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, a believer! It's said that those who respect the spirits and listen to the trees' whispers may be blessed. If you ever venture there, remember to show reverence.");
            }
            else if (speech.Contains("no"))
            {
                Say("Skepticism is healthy. But whether real or imagined, I advise treading with caution. The orchard, spirits or not, can be a treacherous place.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, eager for rewards I see! If you bring me apples from the Whispering Orchard, I shall bake you a special tart and give you a token of my appreciation. A deal?");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BobTheBaker(Serial serial) : base(serial) { }

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
