using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Cantaloupe")]
    public class SirCantaloupe : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirCantaloupe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Cantaloupe";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
            AddItem(new FancyShirt() { Name = "Peach's Royal Tunic", Hue = 1152 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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

            // Initial responses
            if (speech.Contains("name"))
            {
                Say("Greetings, noble traveler! I am Sir Cantaloupe, protector of the peach realm. Have you heard of our grand treasure?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! It is said to be hidden and guarded by valorous knights. To seek it, one must prove their worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must first understand the essence of bravery. What does bravery mean to you?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is more than just facing danger; it is the courage to act with honor. What is honor to you?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the core of a knight’s code. It guides our actions and decisions. What, then, is the knight's code?");
            }
            else if (speech.Contains("code"))
            {
                Say("The knight's code is a set of principles we live by: courage, integrity, and respect. Which of these do you think is most important?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the ability to face fear with resolve. It is crucial in every knight’s journey. How do you cultivate courage?");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity means acting with honesty and strong moral principles. It is vital for a noble quest. How do you uphold integrity?");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect for oneself and others is essential in all interactions. It is the foundation of trust. How do you show respect?");
            }
            else if (speech.Contains("cultivate"))
            {
                Say("To cultivate courage, one must face challenges head-on. What challenges have you faced recently?");
            }
            else if (speech.Contains("uphold"))
            {
                Say("Upholding integrity requires being true to your values even in difficult situations. Have you faced such a situation?");
            }
            else if (speech.Contains("show"))
            {
                Say("Showing respect involves listening and valuing others' perspectives. How do you express appreciation?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are tests of our strength and character. Overcoming them reveals our true selves. What have you learned from your challenges?");
            }
            else if (speech.Contains("situation"))
            {
                Say("Difficult situations test our character and resolve. How did you handle your situation?");
            }
            else if (speech.Contains("appreciation"))
            {
                Say("Expressing appreciation fosters positive relationships and gratitude. What are you grateful for?");
            }
            else if (speech.Contains("learned"))
            {
                Say("From challenges, we learn and grow. Each experience shapes us. What has your journey taught you?");
            }
            else if (speech.Contains("handled"))
            {
                Say("Handling tough situations with grace and wisdom is a sign of growth. What wisdom did you gain?");
            }
            else if (speech.Contains("grateful"))
            {
                Say("Gratitude enriches our lives and perspectives. How do you express your gratitude?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the insight gained from experience. It guides our decisions and actions. How do you apply wisdom in your life?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is unique and filled with lessons. Reflecting on it helps us appreciate our growth. What has been your greatest lesson?");
            }
            else if (speech.Contains("greatest"))
            {
                Say("Greatest lessons are often learned through trials and reflection. What lesson has been most impactful for you?");
            }
            else if (speech.Contains("impactful"))
            {
                Say("Impactful lessons shape our character and actions. For your thoughtful insights and perseverance, accept this token of our kingdom’s gratitude.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new PeachRoyalCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no reward for you right now. Please return later.");
                }
            }

            base.OnSpeech(e);
        }

        public SirCantaloupe(Serial serial) : base(serial) { }

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
