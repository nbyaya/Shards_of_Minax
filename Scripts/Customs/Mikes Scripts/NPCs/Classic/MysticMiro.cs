using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mystic Miro")]
    public class MysticMiro : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MysticMiro() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mystic Miro";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 60;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1908 }); // Dark robe
            AddItem(new Sandals() { Hue = 1908 });
            AddItem(new Spellbook() { Name = "Miro's Mystic Tome" });

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
                Say("Greetings, traveler. I am Mystic Miro.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role in this realm is that of a mystic and a seeker of knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("reflect"))
                {
                    Say("Reflect on these virtues in your journey, for they can guide you toward a path of enlightenment.");
                }
                else
                {
                    Say("The virtues are the cornerstone of a noble life: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
                }
            }
            else if (speech.Contains("miro"))
            {
                Say("Many call me Mystic Miro, but others know me as the guardian of ancient secrets. Have you ever heard of the ancient scrolls?");
            }
            else if (speech.Contains("good"))
            {
                Say("While my physical health remains stable, my spirit constantly seeks balance. In my meditations, I commune with the spirit realm.");
            }
            else if (speech.Contains("mystic"))
            {
                Say("As a mystic, I often aid those in search of deeper understanding. I've recently been studying the lost runes.");
            }
            else if (speech.Contains("scrolls"))
            {
                Say("Ah, the ancient scrolls speak of prophecies and forgotten lore. One such prophecy speaks of a chosen one who will bring balance.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit realm is a place of ethereal beauty and danger. Only those with a pure heart and strong mind can navigate its depths. I can teach you a ritual to glimpse into it, if you wish.");
            }
            else if (speech.Contains("runes"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The lost runes are symbols of ancient magic. Deciphering them can unlock untapped power. I've managed to collect a few. In gratitude for your interest, take this rune as a reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace 'Rune' with the actual item type you want to give
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MysticMiro(Serial serial) : base(serial) { }

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
