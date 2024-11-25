using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Thorne Eldergrove")]
    public class ThorneEldergrove : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ThorneEldergrove() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Thorne Eldergrove";
            Body = 0x191; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 95;
            Hits = 75;

            // Appearance
            AddItem(new WoodlandChest() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandArms() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandGloves() { Hue = Utility.RandomGreenHue() });
            AddItem(new Boots() { Hue = Utility.RandomGreenHue() });

            // Additional items for a forest theme
            AddItem(new QuarterStaff() { Hue = Utility.RandomGreenHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Long hair styles
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, traveler. I am Thorne Eldergrove, guardian of the Enchanted Forest.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, nurtured by the magic of the forest.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect the secrets and treasures of this enchanted woodland.");
            }
            else if (speech.Contains("forest"))
            {
                Say("This forest holds many secrets and ancient magics. Only those who truly seek can uncover them.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The forest whispers its secrets to those who listen carefully. Seek out the hidden groves and ancient trees.");
            }
            else if (speech.Contains("whispers"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromHours(24))
                {
                    Say("You have already received a reward recently. Please return after some time.");
                }
                else
                {
                    Say("You have proven yourself worthy. Accept this Enchanted Forest Chest as a token of your accomplishment.");
                    from.AddToBackpack(new EnchantedForestChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am not familiar with that topic. Perhaps ask me about the forest or its secrets.");
            }

            base.OnSpeech(e);
        }

        public ThorneEldergrove(Serial serial) : base(serial) { }

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
