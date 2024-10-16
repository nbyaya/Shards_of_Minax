using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Flora Thornveil")]
    public class ProfessorFloraThornveil : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasMentionedGarden = false;
        private bool hasMentionedSecrets = false;
        private bool hasMentionedHeart = false;
        private bool hasMentionedSeek = false;
        private bool hasMentionedUnderstanding = false;
        private bool hasMentionedMystical = false;

        [Constructable]
        public ProfessorFloraThornveil() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Flora Thornveil";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new LeafChest() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeafLegs() { Hue = Utility.RandomGreenHue() });
            AddItem(new LeafArms() { Hue = Utility.RandomGreenHue() });
            AddItem(new StuddedGloves() { Hue = Utility.RandomGreenHue() });
            AddItem(new Boots() { Hue = Utility.RandomGreenHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomGreenHue() });

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
                Say("Greetings, I am Professor Flora Thornveil, guardian of the Mystic Garden.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as the flora I tend to.");
            }
            else if (speech.Contains("job"))
            {
                Say("I oversee the growth and enchantment of the plants in this mystical garden.");
            }
            else if (speech.Contains("garden"))
            {
                if (!hasMentionedGarden)
                {
                    hasMentionedGarden = true;
                    Say("Ah, the garden! A place where magic and nature intertwine. The secrets of this garden are known only to a few.");
                }
                else if (hasMentionedSecrets)
                {
                    Say("Indeed, the garden holds many secrets. To truly understand them, one must seek the heart of the garden.");
                }
            }
            else if (speech.Contains("secrets"))
            {
                if (hasMentionedGarden)
                {
                    hasMentionedSecrets = true;
                    Say("The secrets of the garden are hidden deep within its heart. Only those who seek them with a pure heart will uncover them.");
                }
                else
                {
                    Say("Secrets are like whispers in the wind. Only those who have understood the garden can grasp them.");
                }
            }
            else if (speech.Contains("heart"))
            {
                if (hasMentionedSecrets)
                {
                    hasMentionedHeart = true;
                    Say("The heart of the garden is where magic flourishes. It requires more than just seeking; you must truly understand it.");
                }
                else
                {
                    Say("The heart of the garden beats with the rhythm of magic. To find it, one must understand the garden’s essence.");
                }
            }
            else if (speech.Contains("seek"))
            {
                if (hasMentionedHeart)
                {
                    hasMentionedSeek = true;
                    Say("Seeking knowledge is a noble quest. However, it is not just about seeking, but understanding the core of what you seek.");
                }
                else
                {
                    Say("To seek is to embark on a journey of discovery. But remember, the journey should be as enlightening as the destination.");
                }
            }
            else if (speech.Contains("understanding"))
            {
                if (hasMentionedSeek)
                {
                    hasMentionedUnderstanding = true;
                    Say("Understanding the garden's mysteries requires wisdom. The more you comprehend, the closer you are to the truth.");
                }
                else
                {
                    Say("Understanding is the key to unraveling the mysteries. Without it, the journey remains incomplete.");
                }
            }
            else if (speech.Contains("mystical"))
            {
                if (hasMentionedUnderstanding)
                {
                    hasMentionedMystical = true;
                    Say("The garden's mysticism is a reflection of its secrets. To fully grasp its nature, you must delve into the heart and seek with a clear mind.");
                }
                else
                {
                    Say("Mystical forces weave through the garden, influencing all that grows here. Understanding them requires patience and insight.");
                }
            }
            else if (speech.Contains("reveal"))
            {
                if (hasMentionedMystical)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward for you right now. Please return later.");
                    }
                    else
                    {
                        Say("You have shown great understanding and insight. For your efforts, take this Mystic Garden's Cache.");
                        from.AddToBackpack(new MysticGardenCache()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("To reveal the ultimate truth, one must first traverse the path of understanding and insight.");
                }
            }

            base.OnSpeech(e);
        }

        public ProfessorFloraThornveil(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasMentionedGarden);
            writer.Write(hasMentionedSecrets);
            writer.Write(hasMentionedHeart);
            writer.Write(hasMentionedSeek);
            writer.Write(hasMentionedUnderstanding);
            writer.Write(hasMentionedMystical);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasMentionedGarden = reader.ReadBool();
            hasMentionedSecrets = reader.ReadBool();
            hasMentionedHeart = reader.ReadBool();
            hasMentionedSeek = reader.ReadBool();
            hasMentionedUnderstanding = reader.ReadBool();
            hasMentionedMystical = reader.ReadBool();
        }
    }
}
