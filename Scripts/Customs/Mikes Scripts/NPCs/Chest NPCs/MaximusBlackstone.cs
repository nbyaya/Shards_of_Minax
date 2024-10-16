using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Maximus Blackstone")]
    public class MaximusBlackstone : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MaximusBlackstone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Maximus Blackstone";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 95;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = Race.RandomHair(this); // Random hair
            HairHue = Race.RandomHairHue(); // Random hair hue

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
                Say("Greetings, I am Maximus Blackstone, the guardian of ancient knowledge.");
                Say("Ask me about my 'knowledge' or 'secrets'.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("I possess knowledge that few can comprehend. Inquire about 'secrets' or 'treasures'.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I guard are powerful and hidden. If you seek 'treasures', you must first understand 'power'.");
            }
            else if (speech.Contains("power"))
            {
                Say("True power is not just in physical strength but in wisdom and understanding. Ask about 'wisdom' or 'treasures'.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from learning and experience. You must show 'dedication' to earn 'treasures'.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is crucial to achieving one's goals. If you show enough dedication, the 'treasure' shall be yours.");
                Say("Ask about 'treasures' or 'path'.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path to treasure is filled with challenges and learning. Prove your 'dedication' and seek the 'reward'.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Your reward is a token of your success. If you have shown sufficient 'dedication', you may claim it.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Return later to claim another.");
                }
                else
                {
                    Say("For your dedication and perseverance, accept this 'Sith's Vault', a reward of great value.");
                    from.AddToBackpack(new SithsVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are not easily won. Demonstrate 'dedication' and follow the 'path' to claim them.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Prove your worth through 'dedication' and by showing your understanding of 'secrets'.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("If you are dedicated and wise, you will uncover the deepest secrets and treasures.");
            }

            base.OnSpeech(e);
        }

        public MaximusBlackstone(Serial serial) : base(serial) { }

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
