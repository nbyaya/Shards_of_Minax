using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Roland")]
    public class SirRoland : BaseCreature
    {
        [Constructable]
        public SirRoland() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Roland";
            Body = 0x190; // Human male body

            // Stats
            Str = 165;
            Dex = 72;
            Int = 28;
            Hits = 125;

            // Appearance
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new PlateLegs() { Hue = 1175 });
            AddItem(new CloseHelm() { Hue = 1175 });
            AddItem(new PlateGloves() { Hue = 1175 });
            AddItem(new Cloak() { Hue = 1175 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, noble traveler. I am Sir Roland, a humble knight.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is in good condition, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a knight, sworn to uphold the virtues of Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Each day, I strive to live by these virtues. They guide my path as a knight.");
            }
            else if (speech.Contains("follow path virtue"))
            {
                Say("Do you also follow the path of virtue, traveler? What virtues do you hold dear?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of trust. It's the first virtue I learned as a knight, and it's vital in all our interactions. A knight who lies breaks the trust of his peers.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the heart's response to suffering. In our journeys, we often come across those in need. The true test is how we respond. By the way, I once heard that the second syllable of the mantra of Compassion is RAY.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just about bravery in battle, but also standing up for what's right, even when it's difficult. It means confronting fear and injustice with courage.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the cornerstone of a harmonious society. It's not just about punishment, but ensuring that the scales are balanced and fairness prevails.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is about putting others before oneself. As knights, we often must make sacrifices for the greater good. It's a virtue that tests our commitment and love for others.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor binds us as knights. It's more than just our reputation; it's about living with integrity and keeping our word. An honorable knight is respected and trusted.");
            }
            else if (speech.Contains("spirituality"))
            {
                Say("Spirituality connects us with the divine and the mysteries of the universe. It reminds us that there's more to existence than the material world. Prayer and meditation guide me in this virtue.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is recognizing that we are but a small part of the grand tapestry of life. It keeps us grounded and prevents arrogance. It's the virtue that reminds us always to learn and grow.");
            }
            else if (speech.Contains("ray"))
            {
                Say("Ah, you've been listening closely. Indeed, RAY is part of a powerful mantra. It holds a special significance for those who truly understand compassion.");
            }

            base.OnSpeech(e);
        }

        public SirRoland(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
