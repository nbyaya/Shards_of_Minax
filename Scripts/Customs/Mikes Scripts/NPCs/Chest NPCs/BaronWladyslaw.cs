using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baron Władysław de Goldfist")]
    public class BaronWladyslaw : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BaronWladyslaw() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baron Władysław de Goldfist";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2045; // Beard

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
                Say("Greetings, I am Baron Władysław de Goldfist, a noble of the Polish court. Many tales are told of my treasures.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Indeed, my treasures are renowned. But not all treasures are easily found. What more do you wish to know?");
            }
            else if (speech.Contains("found"))
            {
                Say("Finding treasures requires skill and patience. Some say the key lies in understanding the estate.");
            }
            else if (speech.Contains("estate"))
            {
                Say("The estate is vast and filled with history. Seek the hidden chambers if you wish to uncover more.");
            }
            else if (speech.Contains("chambers"))
            {
                Say("The hidden chambers are where secrets are kept. Only those who solve the puzzles can enter.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are guarded fiercely. To find them, one must first learn about the treasures.");
            }
            else if (speech.Contains("learn"))
            {
                Say("To learn about treasures, one must prove their worth by asking the right questions.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Proving your worth involves patience and perseverance. Have you managed to uncover the true nature of the estate?");
            }
            else if (speech.Contains("nature"))
            {
                Say("The nature of the estate is reflected in its artifacts. Seek the chest, and you might find something of great value.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Yes, the chest is a significant part of my treasures. If you prove yourself worthy, you might receive one.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have proven yourself worthy through your inquiries. Accept this Polish Royal Chest as a token of my favor.");
                    from.AddToBackpack(new PolishRoyalChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("value"))
            {
                Say("Value is not always measured in gold. Sometimes, the true worth lies in the journey and knowledge gained.");
            }
            else if (speech.Contains("gold"))
            {
                Say("Gold is but a symbol of wealth. The real treasure lies in the stories and the history behind it.");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of my estate is long and storied. Many artifacts hold pieces of this grand past.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts tell the story of the past. Some say they hold the key to understanding the greatest secrets.");
            }
            else if (speech.Contains("key"))
            {
                Say("The key to the greatest secrets is understanding the estate's history and treasures.");
            }

            base.OnSpeech(e);
        }

        public BaronWladyslaw(Serial serial) : base(serial) { }

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
