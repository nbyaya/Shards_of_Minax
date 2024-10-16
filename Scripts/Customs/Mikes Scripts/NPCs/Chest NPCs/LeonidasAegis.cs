using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Leonidas Aegis")]
    public class LeonidasAegis : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LeonidasAegis() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Leonidas Aegis";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 0x4F6 });
            AddItem(new PlateLegs() { Hue = 0x4F6 });
            AddItem(new PlateArms() { Hue = 0x4F6 });
            AddItem(new PlateGloves() { Hue = 0x4F6 });
            AddItem(new PlateHelm() { Hue = 0x4F6 });
            AddItem(new MetalShield() { Hue = 0x4F6 });

            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = 0x2040; // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204B; // Short beard
            FacialHairHue = Utility.RandomHairHue();

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
                Say("Greetings, I am Leonidas Aegis, a guardian of ancient treasures. My name itself signifies protection.");
            }
            else if (speech.Contains("protection"))
            {
                Say("Yes, protection is paramount. But tell me, what do you know about the valor of warriors?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the essence of true bravery. In the heart of battle, it guides us. But what about our legacy?");
            }
            else if (speech.Contains("legacy"))
            {
                Say("Our legacy is written in the annals of history. To understand it, one must know the tales of ancient Greece.");
            }
            else if (speech.Contains("greece"))
            {
                Say("Greece, the land of myths and legends. Have you heard the stories of the great battles fought here?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Indeed, battles shape the fate of nations. Yet, the wisdom of our ancestors is often forgotten. Have you sought wisdom?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from understanding both victory and defeat. To gain it, one must reflect on their journey. Do you seek such reflection?");
            }
            else if (speech.Contains("reflection"))
            {
                Say("Reflection allows us to grow. It is through introspection that we find our true purpose. Do you strive for purpose?");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose drives us forward, like the strength of a Spartan shield. To honor it, one must demonstrate courage. Are you courageous?");
            }
            else if (speech.Contains("courageous"))
            {
                Say("Courage is tested in moments of dire need. If you are indeed courageous, you must show it in action. Will you accept a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge reveals one's true character. If you accept, you will be tasked with proving your worth. Are you ready?");
            }
            else if (speech.Contains("ready"))
            {
                Say("Very well. To prove your readiness, you must first answer a question about ancient Sparta. What was the motto of Sparta?");
            }
            else if (speech.Contains("motto"))
            {
                Say("The motto of Sparta is 'Come back with your shield or on it.' It signifies the honor of a true warrior. What do you think of honor?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the highest virtue. It is the code by which we live and die. To truly honor this, one must seek the Spartan treasure. Do you desire it?");
            }
            else if (speech.Contains("desire"))
            {
                Say("Desire is the spark that ignites our actions. If you truly desire the Spartan Treasure Chest, you must prove your worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have proven your worth, but I cannot reward you at this moment. Return later.");
                }
                else
                {
                    Say("Your journey has been arduous, and you have proven yourself worthy. Accept this Spartan Treasure Chest as your reward.");
                    from.AddToBackpack(new SpartanTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LeonidasAegis(Serial serial) : base(serial) { }

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
