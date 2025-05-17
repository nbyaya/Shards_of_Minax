using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GraceInExileQuest : BaseQuest
    {
        public override object Title { get { return "Grace in Exile"; } }

        public override object Description
        {
            get
            {
                return 
                    "Pardon the abruptness, but time is of the essence. I am Lady Evelyne Ashford, late of Moon, but Britain is my true home. I return now to reclaim my family's honor, tarnished by lies and betrayal. I must reach Britain to perform the Rite of Restoration—a ritual of blood and name. But there are those who would rather I vanish into the sands forever. Will you see me safely home?";
            }
        }

        public override object Refuse { get { return "Then I must find another... but beware, for those who cross House Ashford rarely find peace."; } }
        public override object Uncomplete { get { return "The road grows darker with each step. Do not leave me to face this alone."; } }

        public GraceInExileQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Britain"));
            AddReward(new BaseReward(typeof(CourtierSilkenRobe), "CourtierSilkenRobe – Graceful attire enhancing social skills"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("You have my eternal gratitude. House Ashford shall rise again, and you shall always have a place in my court. Take this robe—it suits one who walks with grace through danger.", null, 0x59B);
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

    public class LadyEvelyneEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GraceInExileQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel()); // As a noblewoman, she deals in finery and jewelry.
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public LadyEvelyneEscort() : base()
        {
            Name = "Lady Evelyne Ashford";
            Title = "Noblewoman in Exile";
            NameHue = 0x83F;
        }

		public LadyEvelyneEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 60, 40);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8403;
            HairItemID = 0x2048;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 0x489 });
            AddItem(new Cloak() { Hue = 0x47E });
            AddItem(new Sandals() { Hue = 0x59D });
            AddItem(new BodySash() { Hue = 0x482 });
            AddItem(new ElegantCollar() { Hue = 0x47E });
            AddItem(new SilverBracelet() { Hue = 0x482 });
            AddItem(new FlowerGarland() { Hue = 0x5BE }); // A noble but exiled look.
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.12)
                {
                    string[] lines = new string[]
                    {
                        "*Evelyne adjusts her sash, eyes scanning the horizon* 'So much has changed... will they even remember me?'",
                        "*She clutches a small medallion* 'My father's legacy was not meant to end in disgrace.'",
                        "'I heard whispers of betrayal... but never imagined it would strike so close to home.'",
                        "*Evelyne glances at you, a flicker of gratitude in her eyes* 'You risk much for me. Know that House Ashford repays its debts.'",
                        "*Her voice drops to a whisper* 'The ritual... it must be performed under the old oak. Only then can the truth be seen.'",
                        "'Beware those who smile too easily in Britain. A serpent's tongue often hides behind silken words.'"
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
