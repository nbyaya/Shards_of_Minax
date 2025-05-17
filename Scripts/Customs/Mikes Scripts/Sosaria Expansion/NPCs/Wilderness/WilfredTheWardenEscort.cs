using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

    public class WilfredWardenQuest : BaseQuest
    {
        public override object Title { get { return "The Warden’s Burden"; } }

        public override object Description
        {
            get
            {
                return 
                    "I’m Wilfred, Warden of the outer farms... or at least I was. I've uncovered something foul—corruption poisoning the heart of West Montor’s fields, and those who profit from it want me dead. I must return to West Montor with this evidence before it’s lost, and I can’t make it alone. Help me reach the town, and the truth might just survive.";
            }
        }

        public override object Refuse { get { return "Then may the fields rot and the guilty run free... I’ll find another way."; } }
        public override object Uncomplete { get { return "They're close... I can feel their eyes. We need to move, now."; } }

        public WilfredWardenQuest() : base()
        {
            AddObjective(new EscortObjective("the town of West Montor"));
            AddReward(new BaseReward(typeof(ResolutionKeepersSash), "ResolutionKeeper's Sash"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("You’ve done more than save a life—you’ve helped sow justice where rot once ruled. Take this sash, a token of steadfast resolve in dire times.", null, 0x59B);
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

    public class WilfredTheWardenEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(WilfredWardenQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public WilfredTheWardenEscort() : base()
        {
            Name = "Wilfred";
            Title = "the Warden";
            NameHue = 0x83F;
        }

		public WilfredTheWardenEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x203C;
            HairHue = 1150;
            FacialHairItemID = 0x2041;
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 0x47E });
            AddItem(new LeatherChest() { Hue = 0x59B });
            AddItem(new LeatherLegs() { Hue = 0x59B });
            AddItem(new LeatherGloves() { Hue = 0x59B });
            AddItem(new BodySash() { Hue = 0x46F, Name = "Warden’s Sash" });
            AddItem(new Boots() { Hue = 0x497 });
            AddItem(new StuddedGorget() { Hue = 0x59B });
            AddItem(new Broadsword() { Movable = false });
            AddItem(new Cloak() { Hue = 0x46F });
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
                        "*Wilfred clenches a small scroll tightly in his fist* 'This—this is what they killed for.'",
                        "'I was just a lawman... Now I’m the only witness.'",
                        "'Stay sharp. They could be waiting at the next bend.'",
                        "'West Montor’s soil used to be clean. Not anymore.'",
                        "*He winces* 'I didn’t know truth would cost this much.'",
                        "'They won’t stop until I’m silenced... or we reach West Montor.'",
                        "*He glances back nervously* 'We’re too close to fail now.'"
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
}
