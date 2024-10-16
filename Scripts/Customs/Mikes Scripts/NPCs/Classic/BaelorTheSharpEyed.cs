using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baelor the Sharp-Eyed")]
    public class BaelorTheSharpEyed : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BaelorTheSharpEyed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baelor the Sharp-Eyed";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 80;
            Int = 90;
            Hits = 110;

            // Appearance
            AddItem(new LongPants(1175));
            AddItem(new FancyShirt(1154));
            AddItem(new Boots(1904));
            AddItem(new Bow { Name = "Baelor's Bow" });
			
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
                Say("I am Baelor the Sharp-Eyed, a master archer!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in top-notch health, my friend!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect these lands with my keen archery skills.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Sharp eyes and steady hands, that's the way of an archer. Do you seek valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Valor is found in every well-aimed shot and in the defense of these lands.");
            }
            else if (speech.Contains("lands"))
            {
                Say("These lands have been my home for generations. They hold secrets and treasures aplenty, but also dangers. Do you wish to learn more about the threats?");
            }
            else if (speech.Contains("threats"))
            {
                Say("Dire wolves, bandits, and dark sorcery have been seen in the woods. But fear not, my arrows find their mark. Have you encountered any bandits?");
            }
            else if (speech.Contains("bandits"))
            {
                Say("The bandits are cowardly, attacking travelers and stealing their belongings. I've heard rumors of a leader among them, named Skar. Have you heard of him?");
            }
            else if (speech.Contains("skar"))
            {
                Say("Skar is a dangerous man with a bounty on his head. Anyone who can bring me proof of his defeat will be generously rewarded. Are you up to the task?");
            }
            else if (speech.Contains("task"))
            {
                Say("Very well. Bring me Skar's insignia as proof, and you shall receive a reward fitting a brave soul. Remember, my friend, sharp eyes and a steady hand will serve you well.");
            }
            else if (speech.Contains("insignia"))
            {
                Say("It's a unique emblem worn by Skar. It has a black raven on it. If you present it to me, I'll know Skar has been defeated.");
            }
            else if (speech.Contains("sample"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here you go. These arrows are specially crafted with precision in mind. May they fly true and aid you in your endeavors.");
                    from.AddToBackpack(new ArcheryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BaelorTheSharpEyed(Serial serial) : base(serial) { }

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
