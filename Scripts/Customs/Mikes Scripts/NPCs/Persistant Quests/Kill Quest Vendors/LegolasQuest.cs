using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    // Legolas' Orc Hunt Quest using the standard objective
    public class LegolasQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title
        {
            get { return "The Hunt of 500 Orcs"; }
        }

        public override object Description
        {
            get
            {
                return "Greetings, traveler. I am Legolas, Warden of the Emerald Glade.\n\n" +
                       "A foul tide of Orcs marches across these lands, staining the soil with their filth. " +
                       "For too long, we have watched them grow bolder, stronger, and more ruthless.\n\n" +
                       "I seek a true warrior, one whose blade knows no hesitation and whose resolve does not waver. " +
                       "Bring down *five hundred* Orcs, and you shall earn more than glory.\n\n" +
                       "Complete this sacred hunt, and I will grant you access to my cache of enchanted quivers—" +
                       "artifacts crafted for master hunters, rangers, and rogues alike.\n\n" +
                       "Return to me only when the Orcs' screams have echoed across the hills, five hundred-fold.";
            }
        }

        public override object Refuse
        {
            get { return "I see. Perhaps you're not yet ready to carry the weight of such a task. Return when your heart is steadied."; }
        }

        public override object Uncomplete
        {
            get { return "The stench of Orc blood does not yet cling to your blade. Keep hunting, brave one."; }
        }

        public override object Complete
        {
            get
            {
                return "You have returned, and I sense the storm of battle still in your wake.\n" +
                       "The Orcs tremble now, their numbers thinned by your relentless fury.\n\n" +
                       "As promised, my shop is now open to you. Within, you'll find powerful quivers, each enchanted to aid those who walk unique paths—" +
                       "be they alchemists, beastmasters, magicians, or assassins.\n\n" +
                       "Spend your coin wisely, hero. You have earned this.";
            }
        }

        public LegolasQuest() : base()
        {
            // Use your standard objective syntax; the system will track 500 Orc kills
            AddObjective(new SlayObjective(typeof(Orc), "Orcs", 5));
            // Optionally add rewards as needed:
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
			AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Magical Quiver Shop"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            // Mark the quest complete in the talent system (binary flag: 1 means complete)
            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.OrcSlayerQuest))
                profile.Talents[TalentID.OrcSlayerQuest] = new Talent(TalentID.OrcSlayerQuest);
            profile.Talents[TalentID.OrcSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Orcs for Legolas!");
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

    // Legolas NPC that gives the quest and becomes a vendor after quest completion.
    public class Legolas : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

		protected override List<SBInfo> SBInfos
		{
			get { return m_SBInfos; }
		}

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLegolas());
        }

        [Constructable]
        public Legolas() : base("Legolas", "Orc Hunter")
        {
        }

        public Legolas(Serial serial) : base(serial)
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

        // When a player attempts to buy items, check the binary talent flag.
        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                Talent talent;
                if (profile.Talents.TryGetValue(TalentID.OrcSlayerQuest, out talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, champion! Browse my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You have not yet proven your might. Return when you have slain 500 Orcs.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LegolasQuest)
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

    // Vendor shop information for Legolas
    public class SBLegolas : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBLegolas()
        {
        }

        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the purchasable orc here
                Add(new AnimalBuyInfo(1, typeof(Orc), 1000, 10, 17, 0));
				
				// Each quiver costs 5000gp; adjust stock quantity or graphic/itemID as needed.
                Add(new GenericBuyInfo(typeof(AlchemistsQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(BeastmastersQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(FishermansQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(HealersQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(HuntersQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(LockpickersQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(MagiciansQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(NinjasQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(ShadowhuntersQuiver), 5000, 20, 0x2FB7, 0));
                Add(new GenericBuyInfo(typeof(WildernessQuiver), 5000, 20, 0x2FB7, 0));
			
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
