using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Roderick")]
    public class CaptainRoderick : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainRoderick() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Roderick";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 70;
            Int = 90;
            Hits = 100;

            // Appearance
            AddItem(new PlateChest() { Hue = 1133 });
            AddItem(new PlateLegs() { Hue = 1113 });
            AddItem(new Bandana() { Hue = 1653 });
            AddItem(new Boots() { Hue = 1553 });
            AddItem(new GoldBracelet() { Hue = 2153 });
            
            HairItemID = 0x204B; // Long hair
            HairHue = 0x5B; // Dark brown hair

            // Speech Hue
            SpeechHue = 1153; // Marine blue

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
                Say("Ahoy! I am Captain Roderick, the master of the high seas.");
            }
            else if (speech.Contains("health"))
            {
                Say("Aye, I am fit as a fiddle and ready to sail!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to navigate these treacherous waters and uncover the secrets of the deep.");
            }
            else if (speech.Contains("sea") || speech.Contains("ocean"))
            {
                Say("The sea is a wondrous place, full of mysteries and treasures waiting to be found.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! A sailor's greatest desire. Many treasures lie beneath the waves, if you know where to look.");
            }
            else if (speech.Contains("fishing"))
            {
                Say("Fishing is not just a pastime; it's an art. The best catches are those that require patience and skill.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Legends say that the greatest treasures are guarded by mythical sea creatures. But I believe any treasure is worth the effort to find.");
            }
            else if (speech.Contains("rewards"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your inquisitive spirit and knowledge of the sea, accept this special reward.");
                    from.AddToBackpack(new AnglersBounty()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Ah, sailor, there is much to discuss. Ask me about the sea, treasure, or fishing, and I shall provide you with wisdom.");
            }

            base.OnSpeech(e);
        }

        public CaptainRoderick(Serial serial) : base(serial) { }

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
