using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Socrates the Questioner")]
    public class SocratesTheQuestioner : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SocratesTheQuestioner() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Socrates the Questioner";
            Body = 0x190; // Human male body

            // Stats
            Str = 68;
            Dex = 52;
            Int = 123;
            Hits = 59;

            // Appearance
            AddItem(new Robe() { Hue = 2209 });
            AddItem(new Sandals() { Hue = 1177 });
            AddItem(new Spellbook() { Name = "Socratic Dialogues" });

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
                Say("Greetings, seeker of wisdom. I am Socrates the Questioner.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is not measured in mere hit points but in the strength of my philosophical convictions.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation? I am a wanderer of the mind, exploring the vast landscapes of thought.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life is a series of questions. Tell me, traveler, what is the most important question you seek an answer to?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your response intrigues me. The pursuit of wisdom is a noble one. Let us contemplate this together.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is more than just knowledge; it's understanding and applying that knowledge in a meaningful way. Have you met other wise souls on your journey?");
            }
            else if (speech.Contains("convictions"))
            {
                Say("Convictions, strong beliefs, guide us in our paths. Even when faced with adversity, they remain our beacon. Tell me, do you hold any strong convictions?");
            }
            else if (speech.Contains("landscapes"))
            {
                Say("These landscapes are not bound by geography or terrain but by the boundaries we set in our minds. Do you find yourself often setting boundaries or challenging them?");
            }
            else if (speech.Contains("contemplation"))
            {
                Say("To contemplate is to engage in deep thought. In your quests, you may find treasures, but the treasure of understanding oneself is priceless. Would you like a token from my own discoveries as a reminder?");
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
                    Say("Here, take this small pendant. It's a symbol of the endless pursuit of knowledge and understanding. Whenever you wear it, may you be reminded of our conversation and the importance of seeking wisdom.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SocratesTheQuestioner(Serial serial) : base(serial) { }

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
