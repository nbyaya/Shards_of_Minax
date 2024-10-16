using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Merlin Mystocles")]
    public class MerlinMystocles : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MerlinMystocles() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Merlin Mystocles";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomBrightHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomBrightHue() });
            AddItem(new Sandals() { Hue = Utility.RandomBrightHue() });
            AddItem(new Spellbook() { Name = "Merlin's Grimoire" });
            
            Hue = Race.RandomSkinHue(); // Skin color
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
                Say("Ah, greetings! I am Merlin Mystocles, the keeper of arcane secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as a dragon's scale, thanks to my magical prowess.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard ancient magical artifacts and impart knowledge to worthy seekers.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane arts are vast and mysterious. Only those who truly seek will find their secrets.");
            }
            else if (speech.Contains("mystocles"))
            {
                Say("Mystocles is a name steeped in magic. It is said that those who speak it are destined for great adventures.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I guard are not easily revealed. They require patience and wisdom to uncover.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Ah, a seeker of adventure! To embark on such a quest, you must prove your worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("Your worthiness is not just about strength but also about knowledge. Seek further and you may find what you seek.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is a gateway to understanding. The deeper your understanding, the more arcane secrets you can uncover.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding requires contemplation. Have you meditated on the nature of magic and its role in our world?");
            }
            else if (speech.Contains("meditated"))
            {
                Say("Meditation reveals truths hidden to the untrained eye. The more you seek within, the more you will find without.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truths are often veiled in mystery. The quest for truth is what makes the journey worthwhile.");
            }
            else if (speech.Contains("mystery"))
            {
                Say("Mysteries abound in the arcane realm. Each revelation leads to new questions, driving the seeker forward.");
            }
            else if (speech.Contains("revelation"))
            {
                Say("A revelation is a breakthrough in understanding. Each one brings you closer to the ultimate truths of magic.");
            }
            else if (speech.Contains("ultimate truths"))
            {
                Say("The ultimate truths are guarded by the deepest magic. Only the most dedicated can hope to grasp them.");
            }
            else if (speech.Contains("dedicated"))
            {
                Say("Dedication to the pursuit of knowledge and truth is the mark of a true seeker. Your journey is far from over.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is a path to self-discovery and enlightenment. Continue on, and you may find the reward you seek.");
            }
            else if (speech.Contains("enlightenment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your time is not yet ripe. Return later when the stars align for your reward.");
                }
                else
                {
                    Say("For your curiosity and spirit of adventure, I bestow upon you the Mage's Arcane Chest. May it aid you in your journey!");
                    from.AddToBackpack(new MagesArcaneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MerlinMystocles(Serial serial) : base(serial) { }

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
