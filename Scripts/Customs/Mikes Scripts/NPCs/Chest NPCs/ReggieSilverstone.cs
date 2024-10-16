using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Reggie Silverstone")]
    public class ReggieSilverstone : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ReggieSilverstone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Reggie Silverstone";
            Body = 0x190; // Male body
            Title = "the Movie Buff";
            Hue = Utility.RandomSkinHue();

            AddItem(new FancyShirt(Utility.RandomBrightHue()));
            AddItem(new LongPants());
            AddItem(new Shoes());
            AddItem(new FeatheredHat(Utility.RandomBrightHue()));

            SetSkill(SkillName.Parry, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    Say("Greetings, I'm Reggie Silverstone, the Movie Buff!");
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to entertain and share the magic of the silver screen with all who come by.");
                }
                else if (speech.Contains("health"))
                {
                    Say("In good spirits, thank you! The magic of cinema keeps me young at heart.");
                }
                else if (speech.Contains("movies"))
                {
                    Say("Ah, movies! They hold stories of adventure, romance, and fantasy. Have you seen the latest blockbuster?");
                }
                else if (speech.Contains("blockbuster"))
                {
                    Say("Blockbusters are the highlight of the season. They're packed with excitement and thrilling moments.");
                }
                else if (speech.Contains("thrilling"))
                {
                    Say("Thrilling indeed! Just like the chest I have for you. It's full of surprises from the golden age of cinema.");
                }
                else if (speech.Contains("chest"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("Only those who truly appreciate the magic of the movies shall receive the chest.");
                    }
                }
                else
                {
                    Say("I'm afraid I don't understand. Can you ask me about movies, blockbusters, or chests?");
                }
            }

            base.OnSpeech(e);
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                DriveInTreasureTrove chest = new DriveInTreasureTrove();
                from.AddToBackpack(chest);
                Say("You've proven yourself to be a true film aficionado! Here is the Drive-in Treasure Trove, packed with cinematic wonders.");
                m_RewardGiven = true;
            }
        }

        public ReggieSilverstone(Serial serial) : base(serial) { }

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
