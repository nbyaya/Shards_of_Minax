using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class PattyMelt : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public PattyMelt() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Patty";
            Title = "Melt";
            Body = 0x191; // Female body
            Hue = Utility.RandomSkinHue();

            AddItem(new FancyDress(Utility.RandomPinkHue()));
            AddItem(new HalfApron(Utility.RandomBrightHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            HairItemID = Race.RandomHair(Female);
            HairHue = Utility.RandomHairHue();

            SetSkill(SkillName.Cooking, 100.0);
            SetSkill(SkillName.TasteID, 100.0);
            SetSkill(SkillName.ItemID, 75.0);
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
                switch (e.Speech.ToLower())
                {
                    case "name":
                        from.SendMessage("The name's Patty Melt, hon. What can I getcha?");
                        break;
                    case "job":
                        from.SendMessage("I've been slingin' hash at this diner for longer than I care to admit.");
                        break;
                    case "health":
                        from.SendMessage("Health? Well, sugar, I'm fit as a fiddle and twice as musical!");
                        break;
                    case "diner":
                        from.SendMessage("This little diner's been servin' up the best burgers in town since the '50s.");
                        break;
                    case "burger":
                        from.SendMessage("Our secret's in the sauce, darlin'. And no, I won't tell ya what's in it!");
                        break;
                    case "sauce":
                        from.SendMessage("Now, now, don't go askin' about trade secrets. But I might share if you prove yourself worthy.");
                        break;
                    case "worthy":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You gotta show some real diner spirit before I can share our secrets, hon.");
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            return from.Skills[SkillName.Cooking].Base >= 50.0 && from.Skills[SkillName.TasteID].Base >= 50.0;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                DinerDelightChest chest = new DinerDelightChest();
                from.AddToBackpack(chest);
                from.SendMessage("Well, bless your heart! You've got the makings of a real short-order cook. Here's a little somethin' special for ya.");
                m_RewardGiven = true;
            }
            else
            {
                from.SendMessage("Sorry, sugar. I've already shared all my secrets with ya.");
            }
        }

        public PattyMelt(Serial serial) : base(serial)
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