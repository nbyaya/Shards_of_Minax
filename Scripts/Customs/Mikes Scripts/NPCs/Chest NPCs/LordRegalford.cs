using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lord Regalford")]
    public class LordRegalford : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LordRegalford() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Regalford";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();

            // Equipment
            AddItem(new PlateChest() { Hue = 1150 });
            AddItem(new PlateLegs() { Hue = 1150 });
            AddItem(new PlateArms() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new PlateGorget() { Hue = 1150 });
            AddItem(new CloseHelm() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });

            // Hair and beard
            HairItemID = 0x203B; // Bald
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2049; // Long beard
            FacialHairHue = Utility.RandomHairHue();

            // Stats
            Str = 100;
            Dex = 75;
            Int = 90;
            Hits = 80;

            // Speech hue
            SpeechHue = 0;
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
                Say("Greetings, I am Lord Regalford, custodian of the Royal Treasury.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as splendid as the treasures I oversee.");
            }
            else if (speech.Contains("job"))
            {
                Say("I manage and protect the royal treasures of Britannia, ensuring their safety and distribution.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, the treasures of Britannia are vast and wondrous. But only the truly worthy shall have the privilege of accessing them.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To prove your worthiness, you must show wisdom and understanding of the royal virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honor, justice, and courage are the foundations of true nobility. Demonstrate these, and you may earn a special reward.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must return later to receive a reward.");
                }
                else
                {
                    Say("Your understanding of virtues has impressed me. Accept this Royal Treasury Chest as a token of your worth.");
                    from.AddToBackpack(new BritainsRoyalTreasuryChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public LordRegalford(Serial serial) : base(serial) { }

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
