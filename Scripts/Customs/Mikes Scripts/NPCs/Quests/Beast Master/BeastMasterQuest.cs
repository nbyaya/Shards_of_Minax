using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
	public class BeastMasterQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "The Beast Master's Challenge"; } }

		public override object Description
		{
			get
			{
				return 
					"Greetings, adventurer! I am the Beast Master, and I have a challenge for you. " +
					"Prove your worth by completing these tasks:\n\n" +
					"1. Slay 10 pigs to show your combat skills.\n" +
					"2. Bring me 10 wool to demonstrate your resource gathering abilities.\n" +
					"Complete these tasks, and I'll grant you access to my exclusive exotic animal shop!";
			}
		}

		public override object Refuse { get { return "Very well. Return when you're ready for the challenge."; } }

		public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep at it!"; } }

		public override object Complete { get { return "Impressive work! You've proven yourself worthy. My exotic animal shop is now open to you!"; } }

		public BeastMasterQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(Pig), "Pigs", 10));
			AddObjective(new ObtainObjective(typeof(Wool), "Wool", 10, 0xDF8));

			AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
			AddReward(new BaseReward(typeof(BeastToken), 1, "Beast Token")); // Add Beast Token as a reward
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You have completed the Beast Master's Challenge!");
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


	public class BeastMaster : MondainQuester
	{
		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBBeastMaster());
		}

		[Constructable]
		public BeastMaster()
			: base("The Beast Master", "")
		{
		}

		public BeastMaster(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = false;
			Race = Race.Human;

			Hue = Utility.RandomSkinHue();
			HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
			HairHue = Utility.RandomHairHue();
			FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
			FacialHairHue = HairHue;
		}

		public override void InitOutfit()
		{
			AddItem(new Backpack());
			AddItem(new Boots(Utility.RandomNeutralHue()));
			AddItem(new LongPants(Utility.RandomNeutralHue()));
			AddItem(new FancyShirt(Utility.RandomNeutralHue()));
			AddItem(new Cloak(Utility.RandomNeutralHue()));
		}

		public override void VendorBuy(Mobile from)
		{
			PlayerMobile player = from as PlayerMobile;
			if (player != null)
			{
				Item token = player.Backpack.FindItemByType(typeof(BeastToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

				if (token != null || masterToken != null)
				{
					token.Delete(); // Consume the token
					SayTo(from, "Welcome to my exotic animal shop!");
					base.VendorBuy(from);
				}
				else
				{
					SayTo(from, "You must have a Beast Token to access my shop.");
				}
			}
		}

		public override Type[] Quests
		{
			get
			{
				return new Type[] 
				{
					typeof(BeastMasterQuest)
				};
			}
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


    public class SBBeastMaster : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBeastMaster()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Cat), 132, 10, 201, 0));
                Add(new AnimalBuyInfo(1, typeof(Dog), 170, 10, 217, 0));
                Add(new AnimalBuyInfo(1, typeof(Horse), 550, 10, 204, 0));
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 631, 10, 291, 0));
                Add(new AnimalBuyInfo(1, typeof(PackLlama), 565, 10, 292, 0));
                Add(new AnimalBuyInfo(1, typeof(Rabbit), 106, 10, 205, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}