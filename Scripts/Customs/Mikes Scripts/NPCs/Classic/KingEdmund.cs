using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of King Edmund")]
    public class KingEdmund : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KingEdmund() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "King Edmund";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1124 }); // Robe with hue 1124
            AddItem(new Sandals() { Hue = 1124 }); // Sandals with hue 1124
            AddItem(new QuarterStaff() { Name = "King Edmund's Staff" });

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
                Say("I am King Edmund, the ruler of this wretched kingdom.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as miserable as my reign.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\" is to sit on this accursed throne and watch my kingdom crumble.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you dare question the authority of a miserable king like me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, a brave soul, aren't you? Let's see how brave you are in the face of misery!");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("This realm was once a land of prosperity and joy, but now shadows have fallen upon it. Tell me, are you here to help or to observe its downfall?");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Yes, my spirit has been broken by the betrayal of those I once called allies. There is a traitor amongst the courtiers, spreading poison with whispers. If you can unmask this traitor, I will reward you.");
            }
            else if (speech.Contains("throne"))
            {
                Say("This very seat has witnessed the rise and fall of many before me. The weight of it is not in its gold or jewels, but in the responsibility and burden it carries. Have you ever felt such a weight upon your shoulders?");
            }
            else if (speech.Contains("help"))
            {
                Say("If you truly wish to assist, seek out the Oracle in the Whispering Woods. She may have insights into the issues plaguing my realm. And if you prove successful, come back to me, and I shall reward you.");
            }
            else if (speech.Contains("traitor"))
            {
                Say("The traitor is cunning, hiding in plain sight. Gather evidence and present it to me. But be wary, for making false accusations can turn the court against you. If you reveal the traitor's identity, I shall grant you an invaluable reward.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("Ah, so you understand the weight of leadership. Sometimes, it's the silent decisions, the ones made in the shadows of the night, that are the heaviest. Have you ever made such a choice?");
            }
            else if (speech.Contains("oracle"))
            {
                Say("The Oracle is a mysterious being, residing deep within the Whispering Woods. Many seek her wisdom, but not all are deemed worthy. Approach with respect, and she might just share her visions with you.");
            }
            else if (speech.Contains("evidence"))
            {
                Say("The evidence may not always be tangible. Sometimes, it's a word misplaced or an action not taken. Observe the courtiers closely. Your intuition might guide you to the truth.");
            }
            else if (speech.Contains("choice"))
            {
                Say("Choices define us, more than our lineage or wealth. It's the path we choose that carves our legacy. And now, you have the choice to save or forsake this kingdom. What will you choose?");
            }
            else if (speech.Contains("visions"))
            {
                Say("Visions of the Oracle are enigmatic. They may not always make sense immediately, but in time, their meaning becomes clear. If she grants you a vision, ponder upon it, and it might just reveal the way.");
            }
            else if (speech.Contains("intuition"))
            {
                Say("Trusting one's gut can be a powerful tool. However, it's essential to balance intuition with reason. And remember, the traitor will do anything to remain hidden, so be cautious in your pursuit.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("Legacies are built over time, with each action, each choice. My legacy seems to be one of ruin, but with your help, perhaps it can change. Together, we can write a new chapter for this kingdom.");
            }
            else if (speech.Contains("ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To ponder is to reflect deeply, to search within for answers. Not all solutions lie outside; sometimes, the key is within. Seek that key, and you might unlock the salvation of this realm. As a token of appreciation for your dedication, take this.");
                    from.AddToBackpack(new MurderRemovalDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("reason"))
            {
                Say("Reason is the guiding light in the darkness of uncertainty. Use it to navigate the complex web of court politics and to unmask the traitor. And always be wary of those who abandon reason for personal gain.");
            }
            else if (speech.Contains("chapter"))
            {
                Say("Each chapter in a kingdom's history has heroes and villains. Your actions will determine which role you play. But know this, every hero faces challenges, and every villain has a reason. Choose wisely.");
            }

            base.OnSpeech(e);
        }

        public KingEdmund(Serial serial) : base(serial) { }

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
