using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alderwood Wainwright")]
    public class AlderwoodWainwright : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasDiscussedForest = false;
        private bool hasDiscussedBounty = false;
        private bool hasDiscussedRespect = false;
        private bool hasDiscussedWisdom = false;
        private bool hasDiscussedUnderstanding = false;

        [Constructable]
        public AlderwoodWainwright() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alderwood Wainwright";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateHelm() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandChest() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandArms() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodlandGloves() { Hue = Utility.RandomGreenHue() });
            AddItem(new Boots() { Hue = Utility.RandomGreenHue() });
            AddItem(new WoodenShield() { Hue = Utility.RandomGreenHue() });

            Hue = Race.RandomSkinHue(); // Skin color
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Alderwood Wainwright, guardian of the forest's secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the prime of health, thanks to the rejuvenating embrace of the woods.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to preserve the balance of nature and to guide travelers through the forest.");
            }
            else if (speech.Contains("forest"))
            {
                if (speech.Contains("job"))
                {
                    Say("Ah, you are curious about the forest. It holds many wonders and mysteries. If you seek its bounty, you must prove your respect for its beauty.");
                    hasDiscussedForest = true;
                }
                else
                {
                    Say("The forest is a place of great beauty and mystery. Do you seek something specific within it?");
                }
            }
            else if (speech.Contains("bounty"))
            {
                if (hasDiscussedForest)
                {
                    Say("Nature's bounty is a treasure to be cherished. Only those who truly understand its value can receive it.");
                    hasDiscussedBounty = true;
                }
                else
                {
                    Say("Ah, nature's bounty is a topic for those who have shown respect for the forest. Have you spoken with me about the forest?");
                }
            }
            else if (speech.Contains("respect"))
            {
                if (hasDiscussedBounty)
                {
                    Say("Respect for the forest is shown through wisdom and patience. Share your thoughts with me, and you may earn a special reward.");
                    hasDiscussedRespect = true;
                }
                else
                {
                    Say("Respect is the first step to understanding nature. Have you inquired about the forest or its bounty?");
                }
            }
            else if (speech.Contains("wisdom"))
            {
                if (hasDiscussedRespect)
                {
                    Say("Wisdom is the light that guides us through the darkness of ignorance. What is your understanding of nature?");
                    hasDiscussedWisdom = true;
                }
                else
                {
                    Say("Wisdom comes from understanding and respect. Have you spoken with me about respect for the forest?");
                }
            }
            else if (speech.Contains("understanding"))
            {
                if (hasDiscussedWisdom)
                {
                    Say("Understanding nature is essential to fully appreciate its bounty. Prove your understanding, and you may receive a special reward.");
                    hasDiscussedUnderstanding = true;
                }
                else
                {
                    Say("Understanding grows from wisdom and respect. Have you discussed these with me yet?");
                }
            }
            else if (speech.Contains("rewards"))
            {
                if (hasDiscussedUnderstanding)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("You must wait before receiving another reward. Come back later.");
                    }
                    else
                    {
                        Say("Your understanding of nature is commendable. For your insight, accept this gift from the forest.");
                        from.AddToBackpack(new NaturesBountyChest()); // Give the reward chest
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("You need to prove your understanding of nature before I can offer you a reward. Have you discussed the keywords I mentioned?");
                }
            }

            base.OnSpeech(e);
        }

        public AlderwoodWainwright(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasDiscussedForest);
            writer.Write(hasDiscussedBounty);
            writer.Write(hasDiscussedRespect);
            writer.Write(hasDiscussedWisdom);
            writer.Write(hasDiscussedUnderstanding);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasDiscussedForest = reader.ReadBool();
            hasDiscussedBounty = reader.ReadBool();
            hasDiscussedRespect = reader.ReadBool();
            hasDiscussedWisdom = reader.ReadBool();
            hasDiscussedUnderstanding = reader.ReadBool();
        }
    }
}
