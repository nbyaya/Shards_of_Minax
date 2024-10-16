using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Eliott Chesterton")]
    public class EliottChesterton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EliottChesterton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Eliott Chesterton";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1251 });
            AddItem(new StuddedChest() { Hue = 1251 });
            AddItem(new ChainCoif() { Hue = 1251 });
            AddItem(new StuddedGloves() { Hue = 1251 });
            AddItem(new Boots() { Hue = 1251 });
            AddItem(new Spellbook() { Name = "Eliott's Journal" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2045); // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2045; // Beard

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
                Say("Greetings, I am Eliott Chesterton, keeper of the chest and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as hearty as a well-kept chest. Thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard and unveil treasures hidden within these lands.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! There are many secrets and rewards if you can decipher them.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like keys. They unlock the true potential of treasure. Speak to me of the chest and you might reveal more.");
            }
            else if (speech.Contains("chest"))
            {
                Say("The chest you seek holds many wonders. But first, show me your knowledge and resolve.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to many doors. Prove your wisdom, and I shall reward you.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Your resolve must be tested. If you show persistence, you may find the true treasure.");
            }
            else if (speech.Contains("persistence"))
            {
                Say("Persistence is the path to unlocking deeper secrets. Now, speak of the true key to the chest.");
            }
            else if (speech.Contains("key"))
            {
                Say("The key to unlocking true treasure is understanding and patience. Keep this in mind.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is a deep concept. It means recognizing the connections between things. You are close to the treasure.");
            }
            else if (speech.Contains("connections"))
            {
                Say("Connections often reveal the hidden truths. Reflect on what you've learned so far.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truths are hidden beneath layers of mystery. Speak of what lies beneath the surface.");
            }
            else if (speech.Contains("mystery"))
            {
                Say("Mystery is a veil that hides reality. To uncover it, one must ask the right questions.");
            }
            else if (speech.Contains("questions"))
            {
                Say("The right questions lead to deeper answers. Tell me what you seek from this journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey reveals much about oneself. If you seek the final reward, you must demonstrate true insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight is the culmination of all knowledge and experience. For your wisdom, accept this Homeward Bound Chest.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new HomewardBoundChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Return later to claim your reward.");
                }
            }
            else
            {
                Say("I do not understand. Please speak of secrets, knowledge, or the chest.");
            }

            base.OnSpeech(e);
        }

        public EliottChesterton(Serial serial) : base(serial) { }

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
