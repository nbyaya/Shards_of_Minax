using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class StoneHeartsQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Stone Hearts";

        public override object Description =>
            "Name’s Valka Ironlure. I made the mistake of digging too deep into the Mountain Stronghold. " +
            "Turns out, waking up ancient golems isn’t a wise choice.\n\n" +
            "Now they’ve started marching toward the villages. Slay **5 Runestone Golems** before they crush someone—or come knocking on my door.";

        public override object Refuse => "Hah. Cowards live longer, I guess.";

        public override object Uncomplete => "You haven’t shattered enough of them. They’re still stomping around nearby hills.";

        public override object Complete => "That's more like it. Maybe now they’ll stop tracking my scent. Take this—I found it in the stronghold. Didn’t dare wear it.";

        public StoneHeartsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RunestoneGolem), "Runestone Golems", 5));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(StoneheartPendant), 1, "Stoneheart Pendant"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You’ve completed 'Stone Hearts'!");
            Owner.PlaySound(CompleteSound);
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

    public class ValkaIronlure : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this)); 
        }        
		
		[Constructable]
        public ValkaIronlure() : base("Valka Ironlure", "Treasure Seeker")
        {
        }

        public ValkaIronlure(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2047;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherGloves());
            AddItem(new StuddedChest());
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots());
            AddItem(new Pickaxe());
        }

        public override Type[] Quests => new Type[] { typeof(StoneHeartsQuest) };

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
