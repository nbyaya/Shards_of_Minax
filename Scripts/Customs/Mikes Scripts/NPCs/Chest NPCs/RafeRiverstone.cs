using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rafe Riverstone")]
    public class RafeRiverstone : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RafeRiverstone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rafe Riverstone";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Sandals() { Hue = Utility.RandomBlueHue() });
            AddItem(new Tunic() { Hue = Utility.RandomGreenHue() });
            AddItem(new Bandana() { Hue = Utility.RandomBlueHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomBlueHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeatherChest() { Hue = Utility.RandomGreenHue() });

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
                Say("Ahoy there! I'm Rafe Riverstone, the keeper of river lore. To begin, tell me: what brings you to the river?");
            }
            else if (speech.Contains("river"))
            {
                Say("Ah, the river! It holds many secrets. Tell me, what do you seek in these waters?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets, yes! Many seek them. To find them, you must first prove your knowledge of the river's tales. What is the most important trait for a river navigator?");
            }
            else if (speech.Contains("navigator"))
            {
                Say("A navigator must be wise and patient. But there's more. The river is a living entity, and it requires respect. How does one show respect to the river?");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is shown through understanding and harmony. If you are wise and patient, the river will reward you. Have you heard of the ancient river spirits?");
            }
            else if (speech.Contains("spirits"))
            {
                Say("The river spirits guide those who are true of heart. They are the keepers of great treasures. To honor them, you must first understand their lore. Do you know any of their ancient names?");
            }
            else if (speech.Contains("names"))
            {
                Say("Ah, the names! They are many and varied. One must learn their stories to gain the river’s favor. Have you ever heard of the legendary Captain Rivermore?");
            }
            else if (speech.Contains("captain rivermore"))
            {
                Say("Captain Rivermore was known for his adventures on these very waters. To earn his respect, you must solve a riddle. What is the essence of a great river adventure?");
            }
            else if (speech.Contains("adventure"))
            {
                Say("The essence of adventure is discovery and courage. Only those who are brave and inquisitive will find the true treasures. Speak of a time when you faced a great challenge.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Facing challenges with courage is the key to overcoming obstacles. But remember, the greatest reward comes to those who are not only brave but also wise. Are you ready for the final test?");
            }
            else if (speech.Contains("test"))
            {
                Say("The final test is to show your worth through both wisdom and bravery. Tell me, what do you value more: the journey or the destination?");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey shapes us and teaches us more than the destination. For those who understand this, the river has a special gift. Have you ever sought a hidden treasure?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! Many seek it, but few understand its true value. The real treasure is the experience and knowledge gained. For your understanding, take this river’s chest as a reward.");
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("You've already received a reward recently. Come back later.");
                }
                else
                {
                    from.AddToBackpack(new RiverRaftersChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RafeRiverstone(Serial serial) : base(serial) { }

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
