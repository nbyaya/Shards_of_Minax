using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cleopatra Phaera")]
    public class CleopatraPhaera : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CleopatraPhaera() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cleopatra Phaera";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FemaleLeatherChest() { Hue = 1157 });
            AddItem(new LeatherLegs() { Hue = 1157 });
            AddItem(new LeatherArms() { Hue = 1157 });
            AddItem(new LeatherGloves() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new FeatheredHat() { Hue = 1157 });
            
            // Random appearance elements
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x203D); // Example hair styles
            HairHue = Utility.RandomHairHue();

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

            // Dialogues
            if (speech.Contains("name"))
            {
                Say("Greetings, I am Cleopatra Phaera, the guardian of ancient secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as healthy as the Nile is long, thanks to the blessings of the gods.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard the treasures of the pharaohs and protect the ancient knowledge.");
            }
            else if (speech.Contains("pharaoh"))
            {
                Say("The pharaohs were powerful rulers of ancient Egypt, their treasures are legendary.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, the treasure is a symbol of the pharaoh's power and wealth. Do you seek it?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek the treasure, you must prove your wisdom and bravery.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to unlocking many secrets. What do you wish to know?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is shown through action and resolve. Are you prepared to face challenges?");
            }
            else if (speech.Contains("ready"))
            {
                Say("Readiness is shown through your actions. Can you face the trials ahead?");
            }
            else if (speech.Contains("trials"))
            {
                Say("The trials are not just of strength but also of intellect. Are you prepared to test your mind?");
            }
            else if (speech.Contains("intellect"))
            {
                Say("Intellect will guide you through the challenges. Do you have the keen mind needed?");
            }
            else if (speech.Contains("keen"))
            {
                Say("A keen mind sees beyond the surface. Show me your insight and you may find the treasure.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight into the ancient ways will be rewarded. Have you deciphered the ancient symbols?");
            }
            else if (speech.Contains("symbols"))
            {
                Say("The symbols tell stories of old. Understanding them is crucial. Have you studied them?");
            }
            else if (speech.Contains("studied"))
            {
                Say("Study of the past brings wisdom. Are you ready to apply what you have learned?");
            }
            else if (speech.Contains("apply"))
            {
                Say("Application of knowledge is the final step. Are you prepared for the final test?");
            }
            else if (speech.Contains("final"))
            {
                Say("The final test will challenge all you have learned. Face it with courage and wisdom.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is essential for success. Have you shown it in your journey?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Your journey has been long. Reflect on your experiences and the treasure may be yours.");
            }
            else if (speech.Contains("reflect"))
            {
                Say("Reflection brings clarity. Look back on your trials and you may see the path forward.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path to the treasure is clear for those who have proven themselves.");
            }
            else if (speech.Contains("clear"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have demonstrated wisdom, bravery, and insight. For your efforts, I bestow upon you the Pharaoh's Treasure.");
                    from.AddToBackpack(new PharaohsTreasure()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thank"))
            {
                Say("You are welcome. May the blessings of the ancient gods be upon you.");
            }

            base.OnSpeech(e);
        }

        public CleopatraPhaera(Serial serial) : base(serial) { }

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
