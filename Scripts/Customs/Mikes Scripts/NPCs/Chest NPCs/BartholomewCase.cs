using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bartholomew Case")]
    public class BartholomewCase : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool talkedAboutJustice = false;
        private bool talkedAboutLegal = false;
        private bool talkedAboutWorth = false;
        private bool talkedAboutProof = false;

        [Constructable]
        public BartholomewCase() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bartholomew Case";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new Doublet() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Shoes() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailLegs() { Hue = Utility.RandomMetalHue() });

            // Optional: Add a small legal-themed accessory
            AddItem(new Spellbook() { Name = "Bartholomew's Legal Tome" });

            // Facial features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, I am Bartholomew Case, a humble keeper of legal secrets. Ask me about 'justice', 'legal', or 'worth' if you seek to learn more.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as sound as a well-argued case, thank you for asking. Speak to me about 'justice' to continue.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am here to ensure that justice is served, and to share my knowledge with worthy individuals. Start with 'justice' to prove your worth.");
            }
            else if (speech.Contains("justice"))
            {
                if (talkedAboutJustice)
                {
                    Say("We have discussed justice already. To proceed, ask about 'legal'.");
                }
                else
                {
                    talkedAboutJustice = true;
                    Say("Justice is the foundation of a fair society. To understand it deeply, you must explore 'legal' matters next.");
                }
            }
            else if (speech.Contains("legal"))
            {
                if (!talkedAboutJustice)
                {
                    Say("You must first discuss 'justice' with me before we talk about 'legal'.");
                }
                else if (talkedAboutLegal)
                {
                    Say("We have covered 'legal' matters. Now, inquire about 'worth' to proceed further.");
                }
                else
                {
                    talkedAboutLegal = true;
                    Say("The law is a complex web. Understanding it is key. To delve deeper, you must discuss 'worth' next.");
                }
            }
            else if (speech.Contains("worth"))
            {
                if (!talkedAboutLegal)
                {
                    Say("Discuss 'legal' matters with me before we proceed to 'worth'.");
                }
                else if (talkedAboutWorth)
                {
                    Say("We have discussed 'worth'. The next step is to prove your understanding by asking about 'proof'.");
                }
                else
                {
                    talkedAboutWorth = true;
                    Say("To prove your understanding of 'worth', you need to demonstrate your grasp on 'proof'.");
                }
            }
            else if (speech.Contains("proof"))
            {
                if (!talkedAboutWorth)
                {
                    Say("You need to discuss 'worth' before talking about 'proof'.");
                }
                else if (talkedAboutProof)
                {
                    Say("You've already asked about 'proof'. To earn a reward, inquire about 'reward'.");
                }
                else
                {
                    talkedAboutProof = true;
                    Say("Proving your understanding is crucial. For your efforts, you are now ready to ask about 'reward'.");
                }
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else if (talkedAboutProof)
                {
                    Say("Your dedication to understanding the law is commendable. For your efforts, accept this briefcase of legal treasures.");
                    from.AddToBackpack(new LawyerBriefcase()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("You must prove your understanding before asking about 'reward'. Discuss 'proof' first.");
                }
            }

            base.OnSpeech(e);
        }

        public BartholomewCase(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(talkedAboutJustice);
            writer.Write(talkedAboutLegal);
            writer.Write(talkedAboutWorth);
            writer.Write(talkedAboutProof);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            talkedAboutJustice = reader.ReadBool();
            talkedAboutLegal = reader.ReadBool();
            talkedAboutWorth = reader.ReadBool();
            talkedAboutProof = reader.ReadBool();
        }
    }
}
