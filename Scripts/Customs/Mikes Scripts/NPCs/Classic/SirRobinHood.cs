using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Robin Hood")]
    public class SirRobinHood : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirRobinHood() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Robin Hood";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 2126 });
            AddItem(new StuddedChest() { Hue = 2126 });
            AddItem(new StuddedGloves() { Hue = 2126 });
            AddItem(new StuddedArms() { Hue = 2126 });
            AddItem(new StuddedGorget() { Hue = 2126 });
            AddItem(new Boots() { Hue = 2126 });
            AddItem(new Bow() { Name = "Sir Robin Hood's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Sir Robin Hood, the 'hero' of the poor. Ha!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? I'm as fit as a fiddle, unlike the poor souls I 'help.'");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? Oh, I rob from the rich and give to the 'needy.' How noble of me.");
            }
            else if (speech.Contains("heroism"))
            {
                Say("True heroism is a fairy tale. Think you can prove me wrong?");
            }
            else if (speech.Contains("yes") && HasAlreadySpoken(30))
            {
                Say("Ha! You're either a fool or a dreamer. Good luck proving your worth.");
            }
            else if (speech.Contains("hero"))
            {
                Say("The title of 'hero' is bestowed upon me by the people, but I wonder if they truly understand the cost of such a title. Have you ever considered the price of heroism?");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("This old fiddle? It's seen better days. It's a reminder of my past, a simpler time when music brought joy. Ever played an instrument?");
            }
            else if (speech.Contains("needy"))
            {
                Say("The 'needy' are more than just people without coin. They have stories, dreams, and fears. Have you ever stopped to listen to their tales?");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To prove oneself is a never-ending journey. Here, take this as a token for your efforts. May it guide you on your path.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("rob"))
            {
                Say("Robbing is not just about taking wealth. It's a statement against the unjust distribution of power. What's your stance on justice?");
            }
            else if (speech.Contains("fairy"))
            {
                Say("Fairy tales are often tales of hope, masking the harsh reality with sugar-coated truths. Do you believe in fairy tales or reality?");
            }

            base.OnSpeech(e);
        }

        private bool HasAlreadySpoken(int entryNumber)
        {
            // Implement logic to check if NPC has already spoken the given entry number
            return false; // Placeholder logic
        }

        public SirRobinHood(Serial serial) : base(serial) { }

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
