using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Reginald Draconis")]
    public class ProfessorDraconis : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ProfessorDraconis() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Reginald Draconis";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Treasure Hunter";

            AddItem(new FancyShirt(Utility.RandomBrightHue()));
            AddItem(new LongPants(Utility.RandomBrightHue()));
            AddItem(new Boots(Utility.RandomBrightHue()));
            AddItem(new TricorneHat(Utility.RandomBrightHue()));

            SetSkill(SkillName.Hiding, 70.0, 100.0);
            SetSkill(SkillName.Stealing, 70.0, 100.0);
            SetSkill(SkillName.Lockpicking, 70.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    Say("Ah, greetings! I am Professor Reginald 'Rex' Draconis, famed treasure hunter and seeker of legendary artifacts.");
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to uncover and secure rare treasures hidden across the lands. Each treasure holds secrets and power.");
                }
                else if (speech.Contains("health"))
                {
                    Say("I am in fine health, though a bit weary from my latest adventure.");
                }
                else if (speech.Contains("treasure"))
                {
                    Say("Ah, treasure! The allure of hidden riches drives my quest. But to earn such treasures, one must prove their worth.");
                }
                else if (speech.Contains("worth"))
                {
                    Say("Proving your worth requires more than just words. Show me your dedication, and perhaps a reward shall follow.");
                }
                else if (speech.Contains("prove"))
                {
                    Say("To prove your worth, you must answer a challenge. Tell me, what do you seek most in a treasure?");
                }
                else if (speech.Contains("riches"))
                {
                    Say("Riches are but one aspect of a treasure. Knowledge, power, and mystery often accompany them.");
                }
                else if (speech.Contains("knowledge"))
                {
                    Say("Knowledge is a powerful treasure in itself. It opens doors and reveals secrets long forgotten.");
                }
                else if (speech.Contains("power"))
                {
                    Say("Power is tempting, but it must be wielded wisely. The true treasure is understanding how to use it.");
                }
                else if (speech.Contains("mystery"))
                {
                    Say("Mystery is what keeps the world intriguing. It drives us to explore and discover the unknown.");
                }
                else if (speech.Contains("challenge"))
                {
                    if (CheckChallengeConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You must demonstrate your resolve and determination before claiming a reward.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private bool CheckChallengeConditions(Mobile from)
        {
            // This is a placeholder for the challenge conditions.
            // You could implement additional checks here if needed.
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                EliteFoursVault chest = new EliteFoursVault();
                from.AddToBackpack(chest);
                from.SendMessage("Congratulations! You have proven yourself worthy. Take this Elite Four's Vault as your reward.");
                m_RewardGiven = true;
            }
        }

        public ProfessorDraconis(Serial serial) : base(serial)
        {
        }

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
