using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Horus Amuletus")]
    public class HorusAmuletus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HorusAmuletus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Horus Amuletus";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1153 });
            AddItem(new PlateLegs() { Hue = 1153 });
            AddItem(new PlateArms() { Hue = 1153 });
            AddItem(new PlateGloves() { Hue = 1153 });
            AddItem(new PlateHelm() { Hue = 1153 });
            AddItem(new GoldNecklace() { Name = "Amulet of Ra" });
            AddItem(new Sandals() { Hue = 1177 });
            
            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = 0x2045; // Short Hair
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, I am Horus Amuletus, guardian of the Pharaoh's treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, safeguarded by the blessings of the gods.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect the ancient relics and guide the worthy through the trials of the tomb.");
            }
            else if (speech.Contains("tomb"))
            {
                Say("The tombs hold many secrets. Only those who prove their wisdom and bravery are worthy of the treasures within.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are revealed to those who ask with sincerity and determination.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to understanding the ancient knowledge and earning the Pharaoh's favor.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures of the Pharaohs are not easily obtained. You must first demonstrate your worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("To prove your worthiness, you must demonstrate your understanding of our ancient ways and respect for the gods.");
            }
            else if (speech.Contains("ways"))
            {
                Say("The ways of the ancients are both mysterious and profound. They involve respect, knowledge, and honor.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect for the gods and the ancient practices is crucial. It is said that those who disrespect the old ways shall not succeed.");
            }
            else if (speech.Contains("gods"))
            {
                Say("The gods of Egypt are many, each with their own domain and influence. To understand them is to gain true insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight comes from both knowledge and experience. The more you learn, the better you will understand the divine mysteries.");
            }
            else if (speech.Contains("divine"))
            {
                Say("Divine mysteries are the hidden truths of the universe, revealed only to those who seek with a pure heart.");
            }
            else if (speech.Contains("truths"))
            {
                Say("The truths of the universe are vast and intricate. They are often hidden in plain sight, awaiting those who truly seek.");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a journey of discovery. Only through diligent pursuit can one uncover the greatest secrets.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey starts with a single step, but it requires persistence and bravery to reach its end.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is not the absence of fear, but the strength to overcome it. True courage is a quality revered by the gods.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage will guide you through the darkest times and help you face the unknown with resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the willpower to continue despite challenges. It is a trait that the Pharaohs greatly valued.");
            }
            else if (speech.Contains("pharaohs"))
            {
                Say("The Pharaohs were not just rulers but also divine intermediaries. Their legacy endures through the artifacts they left behind.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts are imbued with the essence of the Pharaohs. They hold great power and knowledge.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power comes with great responsibility. The true power of the artifacts lies in how they are used.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("With great power comes the responsibility to use it wisely and honorably.");
            }
            else if (speech.Contains("wisely"))
            {
                Say("To use something wisely is to understand its purpose and act with consideration for the consequences.");
            }
            else if (speech.Contains("consequences"))
            {
                Say("Every action has consequences. Consider your decisions carefully to ensure they align with your goals.");
            }
            else if (speech.Contains("goals"))
            {
                Say("Your goals shape your path. Pursue them with determination and integrity.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the alignment of your actions with your values. It builds trust and respect among those you encounter.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is earned through consistent and honorable behavior. It is a key element in all relationships.");
            }
            else if (speech.Contains("relationships"))
            {
                Say("Relationships are built on mutual respect and understanding. They are integral to a fulfilling life.");
            }
            else if (speech.Contains("fulfilling"))
            {
                Say("A fulfilling life is one lived with purpose and satisfaction, achieved through understanding and connection.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose gives meaning to your actions and guides your journey through life.");
            }
            else if (speech.Contains("journey"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your reward must wait. Return after some time.");
                }
                else
                {
                    Say("Your journey through this dialogue has demonstrated your understanding and perseverance. Accept this Pharaoh's Reliquary as a token of your achievement.");
                    from.AddToBackpack(new PharaohsReliquary()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public HorusAmuletus(Serial serial) : base(serial) { }

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
