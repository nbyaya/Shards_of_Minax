using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Prince Seokjin")]
    public class PrinceSeokjin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PrinceSeokjin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Prince Seokjin";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            // Clothing
            AddItem(new FancyShirt() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Shoes() { Hue = Utility.RandomNeutralHue() });

            // Facial features
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Prince Seokjin, the guardian of ancient treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as the ancient walls of my kingdom.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am tasked with protecting and sharing the wealth of the Three Kingdoms with those who prove their worth.");
            }
            else if (speech.Contains("three kingdoms"))
            {
                Say("The Three Kingdoms were a period of great culture and strength. Only the worthy shall uncover the treasures of this era.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy is to demonstrate both wisdom and courage. Show me your resolve and I shall reward you.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the path to greatness. It requires patience and perseverance. But are you prepared for the trials?");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials test our character. Overcoming them reveals one's true nature. Speak more of your preparation.");
            }
            else if (speech.Contains("preparation"))
            {
                Say("Preparation involves knowledge and strategy. Have you researched the history of the Three Kingdoms?");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of the Three Kingdoms is filled with wisdom and legend. What aspect interests you most?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the guiding light in dark times. It is earned through experience and reflection. Have you sought wise counsel?");
            }
            else if (speech.Contains("wise counsel"))
            {
                Say("Wise counsel often comes from those who have seen much and learned deeply. Seek out those who have lived through great events.");
            }
            else if (speech.Contains("events"))
            {
                Say("Great events shape the course of history. They are the milestones of our existence. Reflect upon these to gain deeper understanding.");
            }
            else if (speech.Contains("reflection"))
            {
                Say("Reflection brings clarity. It is through contemplation that one gains insight. Have you considered your own journey?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its challenges and rewards. Your path to the treasure is no different. Are you ready to take the next step?");
            }
            else if (speech.Contains("next step"))
            {
                Say("The next step is to prove your worthiness through action. Show me your commitment, and the reward shall be yours.");
            }
            else if (speech.Contains("commitment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The time has not yet come for another reward. Return when the sun has traveled further.");
                }
                else
                {
                    Say("Your commitment has been tested and proven. For your bravery and wisdom, accept this chest, which holds the treasures of the Three Kingdoms.");
                    from.AddToBackpack(new TreasureChestOfTheThreeKingdoms()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is the courage to face danger and adversity. It is a trait highly valued in our realm. What does bravery mean to you?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is facing fears with strength and resolve. It is often tested in the heat of battle or challenge.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity builds character and resilience. It is through overcoming difficulties that we grow stronger.");
            }

            base.OnSpeech(e);
        }

        public PrinceSeokjin(Serial serial) : base(serial) { }

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
