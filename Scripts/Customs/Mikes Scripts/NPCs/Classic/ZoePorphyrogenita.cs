using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zoe Porphyrogenita")]
    public class ZoePorphyrogenita : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZoePorphyrogenita() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zoe Porphyrogenita";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new FancyDress() { Hue = 1281 }); // Golden dress
            AddItem(new GoldNecklace() { Name = "Zoe Porphyrogenita's Necklace" });
            AddItem(new Boots() { Hue = 1281 }); // Matching boots

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

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
                Say("Greetings, traveler. I am Zoe Porphyrogenita.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a scholar of Byzantium, dedicated to the pursuit of knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("ponder"))
                {
                    Say("Do you ponder the virtues, traveler? They are the cornerstone of a noble life.");
                }
                else
                {
                    Say("Honor, compassion, and justice are virtues I hold dear. How do they guide your path?");
                }
            }
            else if (speech.Contains("porphyrogenita"))
            {
                Say("Zoe Porphyrogenita, a name passed down through my family for generations. It means 'born into the purple' in ancient tongues. Our lineage is deeply connected with the Byzantine Empire.");
            }
            else if (speech.Contains("good"))
            {
                Say("My good health is largely due to my daily meditations and adherence to the ancient practices of the Byzantine scholars. I also partake in regular herbal remedies.");
            }
            else if (speech.Contains("byzantium"))
            {
                Say("Ah, Byzantium! The heart of an ancient empire, filled with secrets, history, and knowledge yet to be uncovered. I've dedicated my life to decoding its mysteries and artifacts.");
            }
            else if (speech.Contains("noble"))
            {
                Say("Living a noble life means more than just words. It is about actions, decisions, and sometimes sacrifices. If you ever need guidance on this path, I can offer a token of wisdom.");
            }
            else if (speech.Contains("token"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I am pleased you seek guidance. Take this amulet; it's a replica of an ancient Byzantine artifact. It has brought me clarity in times of doubt. May it aid you on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("lineage"))
            {
                Say("Our family traces its roots back to the very halls of Byzantium. Some say we are descended from emperors, but I pride myself more on the wisdom we've inherited than on titles or crowns.");
            }
            else if (speech.Contains("meditations"))
            {
                Say("Meditations are a powerful tool. They connect us to the ancient energies of the world, allowing us to find balance and perspective. If you're interested, I can teach you a basic chant.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("The artifacts from Byzantium are not just historical items; they hold magic and stories of a time long past. Some are dangerous, others enlightening. I've collected a few in my studies.");
            }
            else if (speech.Contains("chant"))
            {
                Say("The chant goes like this: 'Abyssus abyssum invocat'. It translates to 'Deep calls to deep.' Meditate on these words and feel the ancient energies flow.");
            }
            else if (speech.Contains("studies"))
            {
                Say("My studies have taken me to the furthest reaches of the world, from the catacombs of Byzantium to the lost libraries of Alexandria. Each journey enriches my understanding of the past.");
            }

            base.OnSpeech(e);
        }

        public ZoePorphyrogenita(Serial serial) : base(serial) { }

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
