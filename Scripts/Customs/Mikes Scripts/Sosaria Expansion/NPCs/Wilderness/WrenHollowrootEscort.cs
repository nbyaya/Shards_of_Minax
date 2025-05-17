using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WrenHollowrootQuest : BaseQuest
    {
        public override object Title { get { return "The Ghostlight Path"; } }

        public override object Description
        {
            get
            {
                return
                    "Please... I need help! I'm Wren Hollowroot, a humble herbalist. I followed the will-o'-the-wisps too far into the Haunted Forest, searching for a rare flower said to bloom only under ghostlight. Now I fear the forest itself seeks to keep me. Please, guide me back to the village before it steals my soul.";
            }
        }

        public override object Refuse { get { return "You don't understand... the forest is alive. It won't let me go alone."; } }
        public override object Uncomplete { get { return "Please hurry, the lights are calling again..."; } }

        public WrenHollowrootQuest() : base()
        {
            AddObjective(new EscortObjective("a Haunted Forest"));
            AddReward(new BaseReward(typeof(AlchemistsGroundedBoots), "AlchemistsGroundedBoots"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you, brave soul! These boots are yours now. They kept me safe for many years in wild places. May they serve you just as well.", null, 0x59B);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WrenHollowrootEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(WrenHollowrootQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public WrenHollowrootEscort() : base()
        {
            Name = "Wren Hollowroot";
            Title = "the Lost Herbalist";
            NameHue = 0x83F;
        }

		public WrenHollowrootEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 50, 25);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x203C;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 0x59C });
            AddItem(new LongPants() { Hue = 0x482 });
            AddItem(new HalfApron() { Hue = 0x47E });
            AddItem(new Sandals() { Hue = 0x48F });
            AddItem(new FlowerGarland() { Hue = 0x57D });
            AddItem(new HerbalistStaff());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    string[] lines = new string[]
                    {
                        "*Wren glances nervously around* 'The lights... they’re still watching.'",
                        "*She clutches her apron tightly* 'I thought I could find the Ghostbloom... I was wrong.'",
                        "'The trees whisper at night. They tell stories of those who never left.'",
                        "*Wren shivers* 'Every step I take, it feels like the ground pulls me back.'",
                        "'Please, we must not linger... the forest doesn't forgive trespassers.'",
                        "*She looks up with weary eyes* 'I just want to see my garden again.'"
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }

    public class HerbalistStaff : GnarledStaff
    {
        [Constructable]
        public HerbalistStaff() : base()
        {
            Name = "Staff of Verdant Roots";
            Hue = 0x589;
            Attributes.SpellChanneling = 1;
            Attributes.BonusInt = 2;
            SkillBonuses.SetValues(0, SkillName.Alchemy, 5.0);
        }

        public HerbalistStaff(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
