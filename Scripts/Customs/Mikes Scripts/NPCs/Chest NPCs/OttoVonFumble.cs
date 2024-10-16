using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Otto von Fumble")]
    public class OttoVonFumble : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public OttoVonFumble() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Otto von Fumble";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Puzzle Master";

            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGorget() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomMetalHue() });

            SetSkill(SkillName.Parry, 70.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 100.0);
            SetSkill(SkillName.Swords, 70.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, I am Otto von Fumble, the keeper of riddles and secrets.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard the secrets of Bismarck's treasure. To earn it, you must solve my riddles.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, but always ready for a challenge!");
            }
            else if (speech.Contains("puzzle") || speech.Contains("riddle"))
            {
                Say("Here is your first challenge: What has keys but can't open locks?");
            }
            else if (speech.Contains("piano"))
            {
                Say("Correct! Now, for the second challenge: I speak without a mouth and hear without ears. I have no body, but I come alive with wind. What am I?");
            }
            else if (speech.Contains("echo"))
            {
                Say("Well done! Here is your final challenge: The more of this there is, the less you see. What is it?");
            }
            else if (speech.Contains("darkness"))
            {
                if (!m_RewardGiven)
                {
                    Say("Congratulations, traveler! You have solved all my riddles. As a reward, accept this Bismarck's Treasure Chest.");
                    from.AddToBackpack(new BismarcksTreasureChest()); // Reward the player
                    m_RewardGiven = true;
                }
                else
                {
                    Say("You have already received your reward. Return if you wish to solve more riddles.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public OttoVonFumble(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
