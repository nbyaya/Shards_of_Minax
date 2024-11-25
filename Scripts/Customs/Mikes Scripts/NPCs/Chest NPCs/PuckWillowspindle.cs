using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Puck Willowspindle")]
    public class PuckWillowspindle : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PuckWillowspindle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Puck Willowspindle";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 80;
            Int = 100;
            Hits = 50;

            // Appearance
            AddItem(new LeafChest() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeafLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeafArms() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeafGloves() { Hue = Utility.RandomGreenHue() });
            AddItem(new ThighBoots() { Hue = Utility.RandomGreenHue() });
            AddItem(new Cap() { Hue = Utility.RandomGreenHue() });
            AddItem(new Cloak() { Hue = Utility.RandomGreenHue() });

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
                Say("Greetings! I am Puck Willowspindle, the guardian of hidden magic.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Ah, magic! It flows through the very air we breathe. What would you like to know about it?");
            }
            else if (speech.Contains("flow"))
            {
                Say("The flow of magic is like a river—ever-changing and full of surprises. It can bring joy or mischief.");
            }
            else if (speech.Contains("river"))
            {
                Say("Rivers are the lifeblood of the land, just as magic is the lifeblood of our world. Have you ever seen a river enchant a forest?");
            }
            else if (speech.Contains("forest"))
            {
                Say("Forests hold many secrets. They are home to ancient creatures and hidden realms. Do you seek something in the forest?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like hidden treasures. They are often found where least expected, like in a whisper of the wind or a glimmer in the leaves.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures can be physical or mystical. Some are hidden, while others are revealed through courage and wisdom.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the heart of a true adventurer. It allows one to face the unknown and uncover hidden marvels.");
            }
            else if (speech.Contains("marvels"))
            {
                Say("Marvels are wonders that captivate the soul. They can be found in the most unexpected places. Do you wish to see one?");
            }
            else if (speech.Contains("see"))
            {
                Say("To see a marvel, one must first believe in its existence. Only then will the veil of reality reveal its wonders.");
            }
            else if (speech.Contains("believe"))
            {
                Say("Belief is a powerful force. It can shape reality and unlock the doors to hidden realms. Do you have belief in magic?");
            }
            else if (speech.Contains("unlock"))
            {
                Say("Unlocking hidden realms requires both belief and determination. If you have both, then you are ready for a special reward.");
            }
            else if (speech.Contains("realms"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("As you have shown belief and courage, I present you with the Pixie Dust Chest. It holds magical treasures for those who seek.");
                    from.AddToBackpack(new PixieDustChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a journey of discovery. It involves curiosity and an open heart. What do you seek in the world?");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the essence of adventure. It involves uncovering the unknown and finding beauty in unexpected places.");
            }
            else if (speech.Contains("beauty"))
            {
                Say("Beauty can be found in the simplest things—a sunset, a smile, or a hidden gem. What is the most beautiful thing you have seen?");
            }
            else if (speech.Contains("gem"))
            {
                Say("Gems are like magic—shining with their own light and full of potential. They can be physical treasures or metaphorical ones.");
            }
            else if (speech.Contains("potential"))
            {
                Say("Potential is the promise of what could be. It drives us to strive for more and to uncover the hidden possibilities in life.");
            }

            base.OnSpeech(e);
        }

        public PuckWillowspindle(Serial serial) : base(serial) { }

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
