using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Caius Brutus")]
    public class CaiusBrutus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaiusBrutus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Caius Brutus";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1154 });
            AddItem(new PlateLegs() { Hue = 1154 });
            AddItem(new PlateArms() { Hue = 1154 });
            AddItem(new PlateGloves() { Hue = 1154 });
            AddItem(new PlateHelm() { Hue = 1154 });
            AddItem(new MetalShield() { Hue = 1154 });

            Hue = Race.RandomSkinHue(); // Random skin hue
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
                Say("Salve, I am Caius Brutus, guardian of ancient Roman secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, as befits a guardian of history.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to protect and safeguard the treasures of the Roman Empire.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, the treasures of Rome are vast and legendary. Many have sought them, few have found them.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I guard are not easily revealed. They require wisdom and patience.");
            }
            else if (speech.Contains("vault"))
            {
                Say("The vault you seek is filled with wonders of the Roman world. Prove your worth to unlock it.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must first understand the value of perseverance. What do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to journey towards understanding. What drives your quest?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest requires dedication. What knowledge do you seek in your journey?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to many doors. What truths do you seek?");
            }
            else if (speech.Contains("truths"))
            {
                Say("The truth is often hidden beneath layers of mystery. What are you prepared to uncover?");
            }
            else if (speech.Contains("mystery"))
            {
                Say("Mystery invites exploration. Have you the courage to delve deeper?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the heart's response to the unknown. Are you ready to face the challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are but trials of one's resolve. Do you have what it takes?");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return later to test your resolve.");
                }
                else
                {
                    Say("You have shown great resolve and persistence. For your efforts, accept this Roman Emperor's Vault as your reward.");
                    from.AddToBackpack(new RomanEmperorsVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CaiusBrutus(Serial serial) : base(serial) { }

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
