using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hrolf the Stalwart")]
    public class HrolfTheStalwart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HrolfTheStalwart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hrolf the Stalwart";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new NorseHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new RingmailGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new HeaterShield() { Hue = Utility.RandomMetalHue() });
            AddItem(new VikingSword() { Name = "Hrolf's Blade" });

            Hue = Race.RandomSkinHue(); // Adjust as needed
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
            if (!from.InRange(this, 3)) return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Hrolf the Stalwart, a true warrior of the Norse lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am strong and ready for battle, thanks to the gods.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard these lands and seek out the finest treasures for our people.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! A true warrior's reward. But first, you must prove your worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, answer me this: What is the glory of Valhalla?");
            }
            else if (speech.Contains("valhalla"))
            {
                Say("Valhalla is the hall of the slain, where the bravest warriors gather. Only the valiant find their place there.");
            }
            else if (speech.Contains("valiant"))
            {
                Say("Indeed, only the valiant make their mark. What makes a warrior truly valiant?");
            }
            else if (speech.Contains("warrior"))
            {
                Say("A warrior is defined not just by strength, but by honor and courage. Do you seek honor?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a virtue that guides a warrior’s path. Do you know of the ancient rites of honor?");
            }
            else if (speech.Contains("rites"))
            {
                Say("The ancient rites are ceremonies of respect and bravery. To honor the gods, what must a warrior do?");
            }
            else if (speech.Contains("ceremonies"))
            {
                Say("Ceremonies are rituals to earn favor from the gods. What is the most sacred ceremony for the Norse?");
            }
            else if (speech.Contains("sacred"))
            {
                Say("The most sacred ceremony is the rite of passage into Valhalla. Do you seek passage to Valhalla?");
            }
            else if (speech.Contains("passage"))
            {
                Say("Passage to Valhalla is earned through valor and deeds. What deeds do you offer for such honor?");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Great deeds are those that prove one's worth in battle. Have you ever faced a great challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge tests a warrior’s true strength. What was the greatest challenge you faced?");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength alone is not enough. One must also possess wisdom and bravery. Do you possess both?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom guides a warrior's actions. Have you learned the lessons of wisdom through experience?");
            }
            else if (speech.Contains("lessons"))
            {
                Say("Lessons learned in battle are invaluable. What lesson has served you best?");
            }
            else if (speech.Contains("battle"))
            {
                Say("The battlefield is a place of growth and trials. What is the greatest trial you’ve overcome?");
            }
            else if (speech.Contains("trial"))
            {
                Say("Overcoming trials strengthens the spirit. For your trials and wisdom, you have proven your worth.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your valor has not gone unnoticed. Return in time for another reward.");
                }
                else
                {
                    Say("For your dedication and valor, accept this Viking Chest as your reward.");
                    from.AddToBackpack(new VikingChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Speak the words of honor and valor, and you shall earn the right to receive a reward.");
            }

            base.OnSpeech(e);
        }

        public HrolfTheStalwart(Serial serial) : base(serial) { }

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
