using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baron Aleksander Winghart")]
    public class BaronWinghart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BaronWinghart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baron Aleksander Winghart";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1150 });
            AddItem(new PlateLegs() { Hue = 1150 });
            AddItem(new PlateArms() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new PlateHelm() { Hue = 1150 });
            AddItem(new MetalShield() { Hue = 1150 });

            // Random hues
            Hue = Race.RandomSkinHue(); // Adjust to a suitable color if needed
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, I am Baron Aleksander Winghart, a proud descendant of the Winged Hussars.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, ready to recount tales of heroism and honor.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard and preserve the history of the Winged Hussars, and to share their legendary tales.");
            }
            else if (speech.Contains("winged hussars"))
            {
                Say("The Winged Hussars were a formidable cavalry unit known for their bravery and striking appearance. They were legendary in their time.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is the heart of the Winged Hussars. They charged into battle with unyielding courage. Their deeds are celebrated in many tales.");
            }
            else if (speech.Contains("legendary tales"))
            {
                Say("Many stories are told of the Winged Hussars. Seek the truth behind their legendary armor and the treasures they guarded. Their legacy is rich and profound.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, treasures of the Winged Hussars are rare indeed. They were known to guard precious items with their lives. Prove your worthiness, and you might be rewarded.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("To prove worthiness, one must demonstrate knowledge and respect for the Winged Hussars' legacy. Are you ready to learn more about their valor?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor was the essence of the Winged Hussars. Their valor was unmatched on the battlefield. Many sought to emulate their courage.");
            }
            else if (speech.Contains("emulate"))
            {
                Say("To emulate the Winged Hussars is to honor their memory by exhibiting bravery and strength. It is a noble pursuit. Are you prepared for the challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("The challenge is to embody the spirit of the Winged Hussars. Reflect on their virtues, and you may be deemed worthy of their treasure.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues of the Winged Hussars include honor, courage, and loyalty. Embrace these virtues, and you may gain insight into their legendary treasures.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor was the cornerstone of the Winged Hussars' code. They fought with integrity and a sense of duty. True honor is demonstrated through action and character.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the unwavering adherence to moral principles. The Winged Hussars held themselves to the highest standards of integrity, both in battle and in peace.");
            }
            else if (speech.Contains("standards"))
            {
                Say("High standards were essential to the Winged Hussars. They set an example for all, with discipline and a commitment to excellence.");
            }
            else if (speech.Contains("discipline"))
            {
                Say("Discipline was crucial to the Winged Hussars. It ensured they were always prepared for battle and upheld their code of conduct.");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Being prepared was more than just physical readiness; it was about mental and emotional fortitude. The Winged Hussars were always ready to face any challenge.");
            }
            else if (speech.Contains("fortitude"))
            {
                Say("Fortitude is the strength of mind that enables one to endure adversity. The Winged Hussars were renowned for their unwavering fortitude in the face of danger.");
            }
            else if (speech.Contains("endurance"))
            {
                Say("Endurance was a key trait of the Winged Hussars. They could withstand the harshest conditions and continue their noble mission with steadfast resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the firm determination to achieve a goal. The Winged Hussars' resolve was legendary, driving them to overcome every obstacle in their path.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Overcoming obstacles was a testament to the Winged Hussars' strength and resolve. They faced many challenges and emerged victorious.");
            }
            else if (speech.Contains("victorious"))
            {
                Say("Victory was the ultimate reward for the Winged Hussars' valor and dedication. To be victorious is to honor their legacy.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of the Winged Hussars lives on through their heroic deeds and the treasures they protected. To truly honor their legacy, one must understand and respect their history.");
            }
            else if (speech.Contains("history"))
            {
                Say("Understanding history is crucial to appreciating the valor and sacrifice of the Winged Hussars. Their history is rich with heroic deeds and noble ideals.");
            }
            else if (speech.Contains("noble"))
            {
                Say("Noble deeds define the Winged Hussars. Their acts of bravery and honor set them apart as paragons of virtue.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtue is a quality that embodies moral excellence. The Winged Hussars exemplified virtue in every aspect of their lives.");
            }
            else if (speech.Contains("excellence"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your journey through the virtues of the Winged Hussars has been impressive. For your dedication and respect for their legacy, accept this Winged Hussars Chest.");
                    from.AddToBackpack(new WingedHusChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BaronWinghart(Serial serial) : base(serial) { }

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
