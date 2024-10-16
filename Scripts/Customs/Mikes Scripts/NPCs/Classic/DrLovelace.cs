using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Lovelace")]
    public class DrLovelace : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrLovelace() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Lovelace";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 70;
            Int = 125;
            Hits = 58;

            // Appearance
            AddItem(new Skirt() { Hue = 1902 });
            AddItem(new FemaleLeatherChest() { Hue = 1155 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new Bonnet() { Hue = 1900 });
            AddItem(new Spellbook() { Name = "Dr. Lovelace's Algorithms" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // Female
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
                Say("Greetings, traveler. I am Dr. Lovelace, a humble scientist.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as my curiosity.");
            }
            else if (speech.Contains("job"))
            {
                Say("My life's work is the pursuit of knowledge and discovery.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Science teaches us about the world and the forces that govern it. Do you value knowledge, young one?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, a seeker of knowledge! I sense the spark of curiosity within you. Remember, the pursuit of truth is a noble path.");
            }
            else if (speech.Contains("lovelace"))
            {
                Say("Ah, you've heard of me? I've dedicated many years to my studies, delving into realms unknown to most.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Indeed, it is. A sharp mind is bolstered by a healthy body. The wonders of alchemy and potions keep me at my peak.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("The world is filled with mysteries, waiting for eager minds to uncover. Just recently, I stumbled upon an ancient artifact that has baffled many.");
            }
            else if (speech.Contains("studies"))
            {
                Say("I've traversed vast continents, delved into ancient tombs, and met with scholars from distant lands. There's always more to learn.");
            }
            else if (speech.Contains("alchemy"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, alchemy! The fusion of science and magic. I've created many potions and elixirs. In fact, for someone as inquisitive as you, here's a little gift. May it serve you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("artifact"))
            {
                Say("This artifact is said to belong to an ancient civilization. Deciphering its true purpose is my current endeavor.");
            }
            else if (speech.Contains("tombs"))
            {
                Say("Many tombs hold secrets beyond just the remains of the deceased. I've found inscriptions, maps, and relics that tell stories of days long past.");
            }

            base.OnSpeech(e);
        }

        public DrLovelace(Serial serial) : base(serial) { }

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
