using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Terra Branford")]
    public class TerraBranford : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TerraBranford() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Terra Branford";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 120;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 2049 }); // Robe with specified hue
            AddItem(new Boots() { Hue = 1153 }); // Boots with specified hue
            AddItem(new QuarterStaff() { Name = "Terra's Staff" }); // Staff with specific name

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this); // Hair based on gender
            HairHue = Race.RandomHairHue();

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
                Say("I am Terra Branford, torn between two worlds.");
            }
            else if (speech.Contains("health"))
            {
                Say("My heart is uneasy, but my spirit is unbroken.");
            }
            else if (speech.Contains("job"))
            {
                Say("I seek understanding and a place where I belong.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("The path to spirituality is paved with sacrifice and understanding. Dost thou seek understanding?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then thy journey hath just begun. Walk the path with a humble heart.");
            }
            else if (speech.Contains("no"))
            {
                Say("Then may thy path lead thee to enlightenment.");
            }
            else if (speech.Contains("worlds"))
            {
                Say("There's a constant battle within me; one side is human, the other is magical. It has both cursed and blessed me.");
            }
            else if (speech.Contains("uneasy"))
            {
                Say("The duality of my nature sometimes torments me. I often ponder about the essence of my being.");
            }
            else if (speech.Contains("belong"))
            {
                Say("I've journeyed through cities and realms, but the question remains - where is home for someone like me?");
            }
            else if (speech.Contains("battle"))
            {
                Say("I've faced foes and fears alike. But the most challenging battle is accepting both parts of me and finding peace.");
            }
            else if (speech.Contains("essence"))
            {
                Say("At times, I feel a powerful magic coursing through my veins, while at others, I experience the vulnerabilities of a human.");
            }
            else if (speech.Contains("home"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Perhaps home isn't a place, but a feeling of belonging and acceptance. Here, take this token for understanding my plight.");
                    from.AddToBackpack(new TalismanSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("peace"))
            {
                Say("To find true peace, one must reconcile with the shadows of the past and the uncertainties of the future.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic has its price. It can be a gift and a curse. Yet, it's part of who I am, and I must embrace it.");
            }
            else if (speech.Contains("token"))
            {
                Say("This token represents my gratitude and the intertwined nature of magic and humanity. Use it well.");
                from.AddToBackpack(new TrackingAugmentCrystal()); // Give the additional reward
            }

            base.OnSpeech(e);
        }

        public TerraBranford(Serial serial) : base(serial) { }

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
