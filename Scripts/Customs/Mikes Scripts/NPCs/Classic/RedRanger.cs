using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Red Ranger")]
    public class RedRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RedRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Red Ranger";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 110;
            Int = 50;
            Hits = 85;

            // Appearance
            AddItem(new PlateLegs() { Hue = 37 });
            AddItem(new PlateChest() { Hue = 37 });
            AddItem(new PlateHelm() { Hue = 37 });
            AddItem(new PlateGloves() { Hue = 37 });
            AddItem(new Boots() { Hue = 37 });
            AddItem(new Broadsword() { Name = "Red Ranger's Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("Greetings, traveler. I am the Red Ranger.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a Red Ranger, a guardian of the land.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is the strength of one's heart. Do you possess it?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Valor is a precious virtue. Stand strong in the face of adversity, my friend.");
            }
            else if (speech.Contains("ranger"))
            {
                Say("Ah, you've heard of my title. The Red Rangers are a rare breed, trained in the ancient arts of combat and magic. There are few of us left.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, maintaining one's health is crucial for a ranger. Our forest rituals and herbal knowledge keep us in peak condition.");
            }
            else if (speech.Contains("guardian"))
            {
                Say("As guardians, we are sworn to protect the innocent and uphold justice. We often patrol the forests, ensuring the safety of its creatures and travelers.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("The ancient arts are a combination of martial prowess and deep-rooted magic. These skills have been passed down through generations of Red Rangers.");
            }
            else if (speech.Contains("forest"))
            {
                Say("Our rituals connect us to the spirit of the forest. Through them, we gain wisdom and the forest's blessings. Would you like a taste of the forest's blessing?");
            }
            else if (speech.Contains("creatures"))
            {
                Say("The creatures of the forest are our allies and friends. From the majestic stag to the smallest squirrel, they all play a part in the balance of nature.");
            }
            else if (speech.Contains("generations"))
            {
                Say("My ancestors were among the first Red Rangers. Their tales of bravery and valor inspire me every day. Their legacy lives on through me.");
            }
            else if (speech.Contains("blessing"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, a brave soul! Here, take this. It is a small token of the forest's magic. Use it wisely.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the correct item
                    lastRewardTime = DateTime.UtcNow;
                }
            }

            base.OnSpeech(e);
        }

        public RedRanger(Serial serial) : base(serial) { }

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
