using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Abyssal Guardian")]
    public class AbyssalGuardian : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AbyssalGuardian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Abyssal Guardian";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 60;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 2159 }); // Use hue to match the Abyssal Plane theme
            AddItem(new Sandals() { Hue = 2159 });
            AddItem(new Cloak() { Hue = 2159 });
            AddItem(new QuarterStaff() { Hue = 2159 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random hair item IDs
            HairHue = Utility.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string speech = e.Speech.ToLower();

            if (!from.InRange(this, 3))
                return;

            if (speech.Contains("name"))
            {
                Say("I am the Abyssal Guardian, keeper of the Abyssal Plane.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, but the abyssal energies are ever-present.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the secrets of the Abyssal Plane.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The Abyssal Plane holds many secrets. One must prove their worth to uncover them.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must demonstrate wisdom and patience.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from understanding the depths of knowledge. Seek the knowledge hidden within the Abyssal Plane.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is key in the Abyssal Plane. Only those who wait and learn can receive the treasures hidden here.");
            }
            else if (speech.Contains("treasures"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The treasures are not available right now. Please return later.");
                }
                else
                {
                    Say("For your wisdom and patience, I present to you the Abyssal Plane Chest. May it bring you fortune.");
                    from.AddToBackpack(new AbyssalPlaneChest()); // Give the Abyssal Plane Chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public AbyssalGuardian(Serial serial) : base(serial) { }

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
