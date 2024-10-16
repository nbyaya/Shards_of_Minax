using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Valorheart")]
    public class SirReginaldValorheart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldValorheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Valorheart";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2045); // Random hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2049, 0x204B); // Random beard

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
                Say("Greetings, I am Sir Reginald Valorheart, protector of the realms.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, ready for any challenge that may arise.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard the treasures of valor and uphold the honor of knighthood.");
            }
            else if (speech.Contains("valorheart"))
            {
                Say("Ah, Valorheart is a name known for bravery and integrity. It is said that valor is the essence of a true warrior.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, treasure awaits those who are worthy. Prove your valor and you shall be rewarded.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy is to demonstrate courage and resolve. Show me your bravery, and the treasure shall be yours.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is not the absence of fear but the strength to face it. How can one show their bravery?");
            }
            else if (speech.Contains("face"))
            {
                Say("To face danger with courage is a mark of true valor. But what of the trials one must endure?");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials are the challenges we face that test our mettle. Are you prepared to confront them?");
            }
            else if (speech.Contains("mettle"))
            {
                Say("Mettle is the spirit and fortitude one displays in adversity. What do you know of the strength within?");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within, from one's resolve and determination. Have you demonstrated such strength?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the unwavering determination to see one's task through. How have you shown your resolve?");
            }
            else if (speech.Contains("task"))
            {
                Say("Every task set before you is a test of your character. What tasks have you undertaken recently?");
            }
            else if (speech.Contains("undertaken"))
            {
                Say("Undertaking a task requires commitment. What drives you to accomplish your goals?");
            }
            else if (speech.Contains("goals"))
            {
                Say("Goals are the objectives we strive to achieve. Have you accomplished any worthy goals?");
            }
            else if (speech.Contains("accomplished"))
            {
                Say("Accomplishing one's goals is a sign of true determination. Have you proven your worth through your actions?");
            }
            else if (speech.Contains("actions"))
            {
                Say("Actions speak louder than words. Show me your actions, and you may earn the reward.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The chest remains unclaimed for now. Return when you have shown more valor.");
                }
                else
                {
                    Say("Your bravery and resolve are commendable. As a reward for your courage, accept this chest of warrior's treasure.");
                    from.AddToBackpack(new TacticsBonusChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am Sir Reginald Valorheart. Ask me about my name, health, job, or valor, and I shall answer.");
            }

            base.OnSpeech(e);
        }

        public SirReginaldValorheart(Serial serial) : base(serial) { }

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
