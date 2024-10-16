using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Percival")]
    public class SirPercival : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirPercival() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Percival";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 70;
            Int = 30;
            Hits = 120;

            // Appearance
            AddItem(new ChainChest() { Hue = 1153 });
            AddItem(new ChainLegs() { Hue = 1153 });
            AddItem(new PlateHelm() { Hue = 1153 });
            AddItem(new PlateGloves() { Hue = 1153 });
            AddItem(new Cloak() { Hue = 1153 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Sir Percival, a knight of virtue.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health and ready to defend the realm.");
            }
            else if (speech.Contains("job"))
            {
                Say("My noble duty is to protect the weak and uphold the virtues of Honesty, Compassion, and Justice.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The eight virtues guide my every action: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
            }
            else if (speech.Contains("strive"))
            {
                Say("Do you strive to uphold these virtues in your own journey?");
            }
            else if (speech.Contains("percival"))
            {
                Say("I hail from the city of Trinsic, known for its paladins and the virtue of Honor. Have you ever been there?");
            }
            else if (speech.Contains("realm"))
            {
                Say("The realm has been relatively peaceful of late, thanks to the vigilance of its defenders. Yet, dark shadows stir in the North. Beware if you venture there.");
            }
            else if (speech.Contains("protect"))
            {
                Say("Protection isn't just about the sword and shield. It's about heart and understanding. One must be willing to understand those they protect, to truly keep them safe.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of all virtues. Without it, one can never truly be righteous. I once met a mage who valued this virtue above all. He resides in Moonglow, teaching young apprentices the importance of being true to one's word.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the light that shines in the darkest of times. It is what drives us to aid others, even when faced with danger. I've heard tales of a healer in Britain who embodies this virtue. Seek her out if you ever need guidance on this path.");
            }
            else if (speech.Contains("justice"))
            {
                Say("True justice isn't just about punishment, but understanding and rehabilitation. The courts of Yew are known to uphold this virtue with the utmost integrity. If you ever find yourself seeking justice, that's where you should head.");
            }
            else if (speech.Contains("journey"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Every journey is unique, filled with trials and triumphs. For those who truly strive to uphold the virtues, rewards are boundless, both in spirit and in material. In fact, as a token of appreciation for your dedication, take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirPercival(Serial serial) : base(serial) { }

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
