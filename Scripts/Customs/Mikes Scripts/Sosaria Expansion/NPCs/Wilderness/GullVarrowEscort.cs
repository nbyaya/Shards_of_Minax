using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GullVarrowQuest : BaseQuest
    {
        public override object Title { get { return "Sins of the Sea"; } }

        public override object Description
        {
            get
            {
                return
                    "Aye, stranger... I’m Captain Gull Varrow, though I’ve shed my sails for good. Least, I thought I had. There’s a map I carry, to riches no man should touch. I aim to hand it off at Grey Inn before old debts drag me to the deep. Keep me breathing to the inn, and I’ll see you rewarded with a chest sweeter than rum and twice as rare.";
            }
        }

        public override object Refuse { get { return "Best you forget we met, friend. Some debts ain't worth sharing."; } }
        public override object Uncomplete { get { return "The tide waits for none, and neither do my shadows. Let’s be off!"; } }

        public GullVarrowQuest() : base()
        {
            AddObjective(new EscortObjective("Grey Inn"));
            AddReward(new BaseReward(typeof(ChocolatierTreasureChest), "Chocolatier's Treasure Chest"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Aye, you’ve the heart of a true sailor. Take this chest, and know the sea owes you one less curse. Fair winds to ye!", null, 0x59B);
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

    public class GullVarrowEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GullVarrowQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public GullVarrowEscort() : base()
        {
            Name = "Captain Gull Varrow";
            Title = "the Retired Corsair";
            NameHue = 0x83F;
        }

		public GullVarrowEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 50, 50);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8401;
            HairItemID = 0x203B;
            HairHue = 1150;
            FacialHairItemID = 0x204B;
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 0x5BE });
            AddItem(new LongPants() { Hue = 0x497 });
            AddItem(new HalfApron() { Hue = 0x59C });
            AddItem(new TricorneHat() { Hue = 0x59B });
            AddItem(new Boots() { Hue = 0x58B });
            AddItem(new Cutlass());
            AddItem(new BodySash() { Hue = 0x47E });
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
                        "*Gull adjusts his hat, eye scanning the horizon* 'Grey Inn’s not far... but nor are they.'",
                        "'You feel that chill? That’s not the wind, mate... that’s them comin’ for me.'",
                        "*He pats a weathered scroll in his coat* 'This map’s cursed gold, but it ain’t for me. Just gotta see it off.'",
                        "'I’ve outsailed krakens and curses, but debts... debts always catch up.'",
                        "'Ever hear a coin scream? I have. Still do, most nights.'",
                        "*He tightens his grip on his cutlass* 'I’ll not go down easy, not while there’s breath in me.'",
                        "'Grey Inn’s got a fire and a drink waitin’. Let’s live to see it, eh?'"
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
