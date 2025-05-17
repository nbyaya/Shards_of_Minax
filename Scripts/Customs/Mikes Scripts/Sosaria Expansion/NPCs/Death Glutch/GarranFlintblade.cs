using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BurnTheBurnersQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Burn the Burners"; } }

        public override object Description
        {
            get
            {
                return
                    "My name is Garran Flintblade. I'm a weapon smith in Death Gultch, but lately... something's off.\n\n" +
                    "The ore from the nearby veins reeks of brimstone, and flames flicker when no wind stirs. " +
                    "I suspect Ash Wraiths from Hell Dungeon are corrupting the iron with cursed embers.\n\n" +
                    "Slay **12 Ash Wraiths** to purify the area and ensure my forge stays sacred.";
            }
        }

        public override object Refuse { get { return "Suit yourself. But when your blade shatters mid-battle, you'll know why."; } }

        public override object Uncomplete { get { return "You haven’t vanquished enough Ash Wraiths. Their presence still taints the mines."; } }

        public override object Complete { get { return "You’ve done it. The heat feels clean again. My grandfather would be proud. Here—take this."; } }

        public BurnTheBurnersQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AshWraith), "Ash Wraiths", 12));

            AddReward(new BaseReward(typeof(Gold), 1200, "1200 Gold"));
            AddReward(new BaseReward(typeof(FlintbladeSeal), 1, "Flintblade’s Forged Seal"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Burn the Burners'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GarranFlintblade : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGarranFlintblade());
        }

        [Constructable]
        public GarranFlintblade()
            : base("Garran Flintblade", "Master Weapon Smith")
        {
        }

        public GarranFlintblade(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048;
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204C;
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new SmithHammer());
            AddItem(new FullApron());
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new Boots());
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BurnTheBurnersQuest)
                };
            }
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

    public class SBGarranFlintblade : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGarranFlintblade()
        {
        }

        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("Flintblade Forged Blade", typeof(FlintbladeForgedBlade), 850, 10, 0x13B9, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(FlintbladeSeal), 400);
            }
        }
    }
}
