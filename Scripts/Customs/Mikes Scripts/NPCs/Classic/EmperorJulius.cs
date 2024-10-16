using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Julius")]
    public class EmperorJulius : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorJulius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Julius";
            Body = 0x190; // Human male body

            // Stats
            Str = 125;
            Dex = 85;
            Int = 90;
            Hits = 85;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1122 });
            AddItem(new LeatherChest() { Hue = 1122 });
            AddItem(new LeatherGloves() { Hue = 1122 });
            AddItem(new LeatherCap() { Hue = 1122 });
            AddItem(new OrderShield() { Name = "Emperor Julius' Shield" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize the last reward time to a past date
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Emperor Julius, ruler of these lands!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to maintain peace and order in this realm.");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of justice guides my decisions. Do you value justice?");
            }
            else if (speech.Contains("yes") && speech.Contains("justice"))
            {
                Say("Then you and I share a common value. Remember, in the balance of justice, all are equal.");
            }
            else if (speech.Contains("lands"))
            {
                Say("These vast lands have been under the rule of my ancestors for generations. We've faced many challenges to keep them united.");
            }
            else if (speech.Contains("thanks"))
            {
                Say("Your kindness is much appreciated. It's rare to find individuals who genuinely care about the well-being of their rulers these days.");
            }
            else if (speech.Contains("realm"))
            {
                Say("The realm is vast and diverse, with many cultures and challenges. My duty is to ensure all feel represented and heard.");
            }
            else if (speech.Contains("decisions"))
            {
                Say("Every decision I make is carefully weighed. The crown bears heavy with the responsibility of maintaining fairness.");
            }
            else if (speech.Contains("ancestors"))
            {
                Say("My ancestors built this empire with dedication and valor. Their legacy is a reminder of what we must uphold.");
            }
            else if (speech.Contains("kindness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Acts of kindness, no matter how small, have the power to change the world. As a token of my gratitude, take this reward for your concern.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("cultures"))
            {
                Say("Embracing and understanding the various cultures within our realm strengthens our bond. It is a treasure to behold such diversity.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("The responsibility I bear is not just mine. I rely on advisors and the wisdom of the people to guide me in ruling justly.");
            }

            base.OnSpeech(e);
        }

        public EmperorJulius(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime); // Save last reward time
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime(); // Load last reward time
        }
    }
}
