using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zara the Starborn")]
    public class ZaraTheStarborn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZaraTheStarborn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zara the Starborn";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 40;
            Int = 150;
            Hits = 60;

            // Appearance
            AddItem(new FancyDress() { Hue = 2951 });
            AddItem(new Circlet() { Hue = 2952 });
            AddItem(new Spellbook() { Name = "Zara's Starbook" });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Zara the Starborn, a traveler from distant galaxies.");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is beyond the concept of health as you know it.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an observer of the cosmic dance, a seeker of wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom lies not in answers but in the questions unasked. What knowledge do you seek?");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Your curiosity is commendable. The path to enlightenment is paved with questions.");
            }
            else if (speech.Contains("galaxies"))
            {
                Say("The galaxies I've traversed are both magnificent and mysterious. There are tales I could share, if you're interested.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence as I perceive it is vast and interconnected, like a web of stars. Each point of light tells a unique story.");
            }
            else if (speech.Contains("observer"))
            {
                Say("Being an observer has led me to countless places and allowed me to witness the ebb and flow of many civilizations. One such place that holds great interest to me is the Nebula of Shadows.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Ah, questions! They're the keys that unlock the doors of understanding. If you help me with a task, I might reward you with one such key.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path is neither straight nor easy. Sometimes it demands sacrifices, but those who persevere find treasures beyond measure. Would you like to embark on such a journey?");
            }
            else if (speech.Contains("tales"))
            {
                Say("There are many tales I could recount. Some filled with wonders, others with warnings. But there's one tale of the Celestial Serpent that few have heard.");
            }
            else if (speech.Contains("story"))
            {
                Say("Each story from the galaxies I've visited holds lessons and wonders. Some speak of lost civilizations, while others of cosmic phenomena. One such story speaks of the Void Chalice.");
            }
            else if (speech.Contains("task"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The task I have for you is simple, yet it requires a discerning heart. Find the Echoing Crystal hidden in this realm and bring it to me. In return, I will bestow upon you a reward.");
                    from.AddToBackpack(new StaminaLeechAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ZaraTheStarborn(Serial serial) : base(serial) { }

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
