using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class ReleaseTheBoundQuest : BaseQuest
    {
        public override bool DoneOnce => true;
        public override object Title => "Release the Bound";

        public override object Description => 
            "Once, I wore the sigil of Exodus. I fought, bled, and buried too many friends under banners laced with lies.\n\n" +
            "Now I seek redemption.\n\n" +
            "Ten Spectral Sentries still patrol the halls of Exodus Castle, enslaved by oath and necromancy.\n" +
            "**Destroy them**. Free them from the torment that binds their souls to that cursed place.";

        public override object Refuse => "I pray you never have to look into the eyes of a friend and see only chains.";
        public override object Uncomplete => "Their souls still cry out. You must end their suffering.";
        public override object Complete => 
            "You’ve done it… I felt their release like a wind through ash. Perhaps now I can sleep again.\n" +
            "Here—take this. It belonged to the one who first warned me of Exodus’ turn.";

        public ReleaseTheBoundQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpectralSentry), "Spectral Sentries", 10));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(FracturedOathstone), 1, "Fractured Oathstone"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Release the Bound'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SerDalenVoss : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this)); 
        }        
		
		[Constructable]
        public SerDalenVoss() : base("Ser Dalen Voss", "Fallen Knight of the Oathless")
        {
        }

        public SerDalenVoss(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(90, 100, 100);
            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2044;
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204D;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe { Hue = 0x497 });
            AddItem(new Sandals { Hue = 0x1BB });
            AddItem(new HoodedShroudOfShadows()); // Could be styled for an "ex-knight turned monk"
        }

        public override Type[] Quests => new Type[] { typeof(ReleaseTheBoundQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
