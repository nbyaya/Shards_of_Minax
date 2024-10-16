using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Priestess Isidora")]
    public class PriestessIsidora : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PriestessIsidora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Priestess Isidora";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 50;
            Int = 120;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Priestess Isidora, a humble servant of the divine.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no consequence, for the spirit endures.");
            }
            else if (speech.Contains("job"))
            {
                Say("I serve as a guardian of sacred knowledge and faith.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtue of Humility teaches us to bow before the mysteries of life. Do you ponder the virtues, traveler?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Then let us contemplate the virtues together. Which do you find most challenging in your own life?");
            }
            else if (speech.Contains("divine"))
            {
                Say("The divine guides my actions and thoughts. Many come here seeking solace and guidance. Do you seek guidance?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit is eternal and untouched by the troubles of the mortal realm. However, the soul can often require healing. Have you encountered any troubled souls on your journey?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("The sacred knowledge I guard is not only about rituals, but also about ancient relics. Some relics have immense power. Are you interested in such relics?");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is but one of the virtues. Each virtue has a shrine dedicated to it in the realm. Have you visited any of these shrines?");
            }
            else if (speech.Contains("challenging"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Embracing virtues can be a lifelong endeavor. As a token of encouragement, please accept this gift from me.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("guidance"))
            {
                Say("Many seek guidance to find their purpose or to make amends. The path to redemption is often paved with good intentions. Have you wronged someone and seek redemption?");
            }
            else if (speech.Contains("souls"))
            {
                Say("Troubled souls often wander the realm, lost and seeking peace. There's a sacred grove nearby that is said to bring solace to these souls. Would you like directions?");
            }
            else if (speech.Contains("relics"))
            {
                Say("The relics are ancient artifacts from a time long gone. They are imbued with great power, but also carry dangers. One must be cautious. Have you encountered any dangers on your quest?");
            }

            base.OnSpeech(e);
        }

        public PriestessIsidora(Serial serial) : base(serial) { }

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
