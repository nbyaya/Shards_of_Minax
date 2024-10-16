using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hua Mulan")]
    public class HuaMulan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HuaMulan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hua Mulan";
            Body = 0x190; // Human female body

            // Stats
            Str = 110;
            Dex = 110;
            Int = 70;
            Hits = 85;

            // Appearance
            AddItem(new LeatherChest() { Hue = 1158 }); // Armor item with hue 1158
            AddItem(new Boots() { Hue = 1158 }); // Boots with hue 1158
            AddItem(new Longsword() { Name = "Mulan's Sword" }); // Weapon

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Hua Mulan, a warrior from the Middle Kingdom.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is strong, for I have endured many battles.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a warrior, dedicated to protecting my people and land.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues are like the wind, guiding us through life's challenges. Do you seek wisdom in them?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then meditate on their meanings, for they hold the keys to a virtuous life.");
            }
            else if (speech.Contains("middle"))
            {
                Say("The Middle Kingdom is a land of beauty and tradition, where rivers and mountains hold tales of ancient legends.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Each battle has taught me lessons of courage and perseverance. Once, I fought against a fearsome dragon to save a village.");
            }
            else if (speech.Contains("protecting"))
            {
                Say("Protecting my people has always been my foremost duty. I once disguised myself as a man to take my ailing father's place in the army.");
            }
            else if (speech.Contains("legends"))
            {
                Say("Legends are not just tales. They are memories of our past, a reflection of our culture. I've heard of a legendary sword that once belonged to a hero of the Middle Kingdom.");
            }
            else if (speech.Contains("dragon"))
            {
                Say("The dragon was mighty, with scales as tough as armor. But with the help of my companions and strategy, we overcame it. For my bravery, I was bestowed a special reward by the village elders.");
            }
            else if (speech.Contains("disguise"))
            {
                Say("In disguise, I faced many challenges, not just on the battlefield but also in maintaining my secret. But my loyal friend, Mushu, always stood by my side.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The reward was a pendant, symbolic of bravery and honor. I can see you possess similar qualities. Take this as a token of my appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("sword"))
            {
                Say("The sword was said to possess magical properties, able to cut through the hardest of substances. Many have sought it, but few have found its resting place.");
            }
            else if (speech.Contains("mushu"))
            {
                Say("Mushu is a spirited dragon guardian, always eager to offer advice, whether I ask for it or not! His humor and wit have saved me from despair on many occasions.");
            }
            else if (speech.Contains("magical"))
            {
                Say("The magical properties of the sword were said to be granted by the gods themselves. Only those pure of heart can wield its true power.");
            }

            base.OnSpeech(e);
        }

        public HuaMulan(Serial serial) : base(serial) { }

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
