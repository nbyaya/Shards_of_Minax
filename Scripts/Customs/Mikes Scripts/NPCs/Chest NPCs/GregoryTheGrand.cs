using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gregory the Grand")]
    public class GregoryTheGrand : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GregoryTheGrand() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gregory the Grand";
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
            HairItemID = Utility.RandomList(0x2040, 0x203B); // Random hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2042, 0x2045); // Random facial hair styles

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
                Say("Ah, I am Gregory the Grand, the keeper of secrets and treasures untold.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the strongest fortress, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard and share the knowledge of rare and valuable artifacts.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, I guard a treasure that many seek but few find.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets, you say? Only those with wisdom and patience can uncover them.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to unlocking the greatest treasures. Have you come to prove your wisdom?");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your wisdom, you must first understand the value of patience and knowledge.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue. One must be patient to unlock the deeper mysteries.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries are unveiled through perseverance and inquiry. Have you the perseverance to seek further?");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance will lead you to greater understanding. What else do you seek to learn?");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding comes from asking the right questions and seeking answers with diligence.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Asking the right questions is crucial. Have you considered the nature of the treasure I guard?");
            }
            else if (speech.Contains("nature"))
            {
                Say("The nature of the treasure is both material and symbolic. It reflects the virtues of the seeker.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honesty, bravery, and integrity are key to unlocking the true value of the treasure.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of trust. It is essential in all interactions and pursuits.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery allows one to face challenges head-on. It is a trait admired by all who seek greatness.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity ensures that one remains true to their principles. It is a virtue that guides one’s actions.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Actions define one’s character. It is through actions that one proves their worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("Worthiness is determined by one’s adherence to virtues and their commitment to the quest.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is the final test of a seeker’s resolve. Do you feel committed to your quest?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is not merely a journey but a path of growth and learning. Are you prepared for the final challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must show commitment over time. Please return later for your reward.");
                }
                else
                {
                    Say("Your commitment and perseverance have proven your worth. Accept this treasure chest as a reward for your journey.");
                    from.AddToBackpack(new SpecialWoodenChestIvan()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public GregoryTheGrand(Serial serial) : base(serial) { }

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
