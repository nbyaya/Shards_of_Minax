using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Qin Shi Huang")]
    public class QinShiHuang : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public QinShiHuang() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Qin Shi Huang";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 80;
            Hits = 90;

            // Appearance
            AddItem(new Robe() { Hue = 1160 });
            AddItem(new Cap() { Hue = 1160 });
            AddItem(new Sandals() { Hue = 1160 });
            AddItem(new Halberd() { Name = "Qin's Glaive" });

            Hue = Race.RandomSkinHue();
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
                Say("Greetings, traveler! I am Qin Shi Huang.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a scholar and a keeper of ancient wisdom.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The eight virtues are the foundation of a noble life: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
            }
            else if (speech.Contains("ponder"))
            {
                Say("Do you ponder these virtues on your journey, traveler?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Ancient wisdom, like the one I keep, is not just about knowing, but about understanding. Wisdom is like a tree whose roots delve deep into history. Would you like to learn more about history?");
            }
            else if (speech.Contains("history"))
            {
                Say("Ah, history! The lessons of the past are invaluable for the future. I particularly enjoy studying the tales of the legendary heroes and the artifacts they used. Have you ever heard of the jade pendant?");
            }
            else if (speech.Contains("pendant"))
            {
                Say("The jade pendant is a symbol of protection and clarity. It was said to be lost in the catacombs beneath this land. Whoever possesses it gains insight beyond mere mortals. I can offer a reward to anyone who returns it to me.");
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
                    Say("Ah, a seeker of treasures! If you were to bring me the jade pendant, I would bestow upon you a piece of knowledge lost to time and an artifact from my collection.");
                    from.AddToBackpack(new BushidoAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("collection"))
            {
                Say("Over the centuries, I have amassed a trove of rare artifacts and scrolls. Each item tells a story, a fragment of the vast tapestry of history. By studying them, one can gain insight into the past and perhaps shape a better future.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is a river's course, shaped by the actions of those in the present. By understanding the virtues and lessons of the past, we can navigate its waters more wisely. Tell me, traveler, what do you seek in your future?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to desire knowledge, growth, and understanding. I admire that in an individual. May your path be illuminated by the light of wisdom, and may the virtues guide your steps.");
            }

            base.OnSpeech(e);
        }

        public QinShiHuang(Serial serial) : base(serial) { }

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
