using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jedi Master Aldric")]
    public class JediMasterAldric : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JediMasterAldric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jedi Master Aldric";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });
            AddItem(new HoodedShroudOfShadows() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Aldric's Tome of Wisdom" });

            // Randomize appearance slightly
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2045); // Example hairstyles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0; // No facial hair for this character

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
                Say("Greetings, I am Jedi Master Aldric. What brings you to these ancient lands?");
            }
            else if (speech.Contains("health"))
            {
                Say("The Force sustains me well. How can I assist you today?");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard the ancient relics and impart wisdom to those who seek it.");
            }
            else if (speech.Contains("relics"))
            {
                Say("The relics I guard are not of this world but of a forgotten era. They hold great power and knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the path to enlightenment. Seek it with an open heart and mind.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment comes from understanding one's self and the universe. It is a journey, not a destination.");
            }
            else if (speech.Contains("reliquary"))
            {
                Say("Ah, the 'Jedi's Reliquary'. A great treasure indeed, but to earn it, one must understand the true meaning of the Force.");
            }
            else if (speech.Contains("force"))
            {
                Say("The Force is an energy field created by all living things. It binds the galaxy together.");
            }
            else if (speech.Contains("energy"))
            {
                Say("Energy flows through everything. Understanding its nature can reveal the deepest secrets of the universe.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The deepest secrets are hidden in the past. The ancient texts can reveal much if you seek them.");
            }
            else if (speech.Contains("texts"))
            {
                Say("The ancient texts speak of a time when the Force was in balance. Seek harmony in your quest.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the balance of light and dark. Understanding this balance is key to mastering the Force.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is not just about opposites. It is about finding peace within the chaos. The Force is a reflection of this balance.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace within oneself is the path to true enlightenment. It allows you to see beyond the illusions of the material world.");
            }
            else if (speech.Contains("illusions"))
            {
                Say("Illusions can deceive the senses. True sight comes from within, guided by the Force.");
            }
            else if (speech.Contains("sight"))
            {
                Say("True sight sees beyond appearances. It perceives the essence of things and the flow of the Force.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the Force is what connects all living things. It is the core of existence and understanding it is crucial.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence is a cycle of life and death, light and dark. Embrace it and you will understand the Force more deeply.");
            }
            else if (speech.Contains("embrace"))
            {
                Say("To embrace the Force is to accept all aspects of life. Only then can you truly be at peace.");
            }
            else if (speech.Contains("accept"))
            {
                Say("Acceptance is key to mastery. By accepting the Force, you accept yourself and the world around you.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery of the Force requires patience and dedication. It is not something achieved overnight, but through constant learning.");
            }
            else if (speech.Contains("learning"))
            {
                Say("Learning is a continuous journey. Each step you take brings you closer to understanding the true nature of the Force.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey to understanding the Force is a path of discovery and growth. Keep searching and you will find what you seek.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the key to unlocking the secrets of the Force. Each new insight brings you closer to the ultimate truth.");
            }
            else if (speech.Contains("truth"))
            {
                Say("The ultimate truth of the Force is beyond words. It must be experienced and understood through the heart and mind.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is the greatest teacher. Through it, you come to understand the deeper aspects of the Force.");
            }
            else if (speech.Contains("teacher"))
            {
                Say("A true teacher guides you to find your own answers. The Force is a guide to those who seek its wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from experience and reflection. The Force is a source of infinite wisdom for those who listen.");
            }
            else if (speech.Contains("listen"))
            {
                Say("To listen to the Force is to hear its subtle guidance. It speaks to those who are attuned to its whispers.");
            }
            else if (speech.Contains("attuned"))
            {
                Say("Attunement to the Force requires patience and meditation. It is through stillness that you can hear its voice most clearly.");
            }
            else if (speech.Contains("meditation"))
            {
                Say("Meditation is a practice of quieting the mind and connecting with the Force. It is essential for any Jedi.");
            }
            else if (speech.Contains("jedi"))
            {
                Say("The Jedi are guardians of peace and knowledge. Through meditation, they maintain their connection to the Force.");
            }
            else if (speech.Contains("guardians"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The Force tells me that I have already bestowed a gift recently. Return later for another chance.");
                }
                else
                {
                    Say("As a token of your quest for knowledge and understanding, accept this 'Jedi's Reliquary' as your reward.");
                    from.AddToBackpack(new JedisReliquary()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public JediMasterAldric(Serial serial) : base(serial) { }

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
