using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Count Vasily Goldov")]
    public class CountVasilyGoldov : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CountVasilyGoldov() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Count Vasily Goldov";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 70;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldBracelet() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldRing() { Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0; // No facial hair

            // Speech Hue
            SpeechHue = 1157; // Gold hue

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
                Say("Greetings, I am Count Vasily Goldov, keeper of treasures and secrets.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard and protect the hidden treasures of the Tsar.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, ready to reward those who prove their worth.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! Many seek it, but only the worthy shall find it.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Here is my challenge: I am full of gold but hold no weight. What am I?");
            }
            else if (speech.Contains("chest"))
            {
                Say("The answer to my riddle is a chest. Solve the riddle to claim your reward.");
            }
            else if (speech.Contains("gold"))
            {
                Say("Gold is but a symbol of the wealth that awaits those who are brave enough to seek it.");
            }
            else if (speech.Contains("wealth"))
            {
                Say("True wealth is not just in gold but in the journey and the bravery required to seek it.");
            }
            else if (speech.Contains("brave"))
            {
                Say("It takes courage to venture into the unknown. Only the brave shall uncover the deepest secrets.");
            }
            else if (speech.Contains("secret"))
            {
                Say("The greatest secrets are often hidden in plain sight. If you have been brave, you might just find the treasure.");
            }
            else if (speech.Contains("chest") && speech.Contains("gold"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Return later to try again.");
                }
                else
                {
                    Say("Well done! For your cleverness, take this Tsar's Treasure Chest as your reward.");
                    from.AddToBackpack(new TsarsTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CountVasilyGoldov(Serial serial) : base(serial) { }

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
