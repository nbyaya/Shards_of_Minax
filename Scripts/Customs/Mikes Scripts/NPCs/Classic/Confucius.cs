using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Confucius")]
    public class Confucius : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Confucius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Confucius";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1154 });
            AddItem(new Sandals() { Hue = 1154 });
            AddItem(new GnarledStaff() { Name = "The Analects" });

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
                Say("Greetings, traveler. I am Confucius.");
            }
            else if (speech.Contains("health"))
            {
                Say("Life's health ebbs and flows like the tides of the sea.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am but a humble philosopher, seeking the path of wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Do you seek wisdom, young one?");
            }
            else if (speech.Contains("no"))
            {
                Say("Then, young one, ponder the riddle of existence and return when you seek my guidance.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Confucius is but a name, for in the end, aren't we all just stardust in the vast expanse of the universe?");
            }
            else if (speech.Contains("tides"))
            {
                Say("The tides are governed by the moon's embrace. As is the health of the land, so too is the soul influenced by unseen forces.");
            }
            else if (speech.Contains("philosopher"))
            {
                Say("A philosopher's work is never done, for with each answer, a dozen new questions arise. Have you encountered any mysteries in your travels?");
            }
            else if (speech.Contains("guidance"))
            {
                Say("True guidance often comes from within, but sometimes, one requires a nudge in the right direction. Would you like a clue to the riddles of existence?");
            }
            else if (speech.Contains("clue"))
            {
                Say("Very well. 'The greatest treasures are those invisible to the eye but felt by the heart.' Reflect upon this, and should you grasp its meaning, I may have a reward for you.");
            }
            else if (speech.Contains("treasures"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you have pondered deeply! As promised, here is a reward for your insightful journey. May it serve you well.");
                    from.AddToBackpack(new DispelAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Confucius(Serial serial) : base(serial) { }

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
