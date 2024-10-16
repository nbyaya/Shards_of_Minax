using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CluckmasterCromwellQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cluckmaster Cromwell's Feathered Fiasco"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave soul! I am Cluckmaster Cromwell, the undisputed authority on all things feathered. " +
                    "To gain access to my exclusive avian shop, you must prove your prowess by dispatching these peculiar poultry: \n\n" +
                    "1. Slay a FireRooster.\n" +
                    "2. Defeat a FrostHen.\n" +
                    "3. Conquer an IllusionHen.\n" +
                    "4. Overcome a MysticFowl.\n" +
                    "5. Destroy a NecroRooster.\n" +
                    "6. Vanquish a PoisonPullet.\n" +
                    "7. Subdue a ShadowChick.\n" +
                    "8. Eradicate a StoneRooster.\n" +
                    "9. Defeat a Thunderbird.\n" +
                    "10. Exterminate a WindChicken.\n\n" +
                    "Complete these tasks, and I will grant you access to my exclusive poultry shop!";
            }
        }

        public override object Refuse { get { return "Come back when you're ready to tackle these feathered foes."; } }

        public override object Uncomplete { get { return "You haven't managed to take down all the required birds yet. Keep at it!"; } }

        public override object Complete { get { return "Fantastic! You've proven your skill against the feathered fiends. My shop is now open to you!"; } }

        public CluckmasterCromwellQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FireRooster), "FireRoosters", 1));
            AddObjective(new SlayObjective(typeof(FrostHen), "FrostHens", 1));
            AddObjective(new SlayObjective(typeof(IllusionHen), "IllusionHens", 1));
            AddObjective(new SlayObjective(typeof(MysticFowl), "MysticFowls", 1));
            AddObjective(new SlayObjective(typeof(NecroRooster), "NecroRoosters", 1));
            AddObjective(new SlayObjective(typeof(PoisonPullet), "PoisonPullets", 1));
            AddObjective(new SlayObjective(typeof(ShadowChick), "ShadowChicks", 1));
            AddObjective(new SlayObjective(typeof(StoneRooster), "StoneRoosters", 1));
            AddObjective(new SlayObjective(typeof(Thunderbird), "Thunderbirds", 1));
            AddObjective(new SlayObjective(typeof(WindChicken), "WindChickens", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(CluckToken), 1, "Cluck Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You've completed Cluckmaster Cromwell's challenge!");
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

    public class CluckmasterCromwell : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCluckmasterCromwell());
        }

        [Constructable]
        public CluckmasterCromwell()
            : base("Cluckmaster Cromwell", "Master of the Feathered Foes")
        {
        }

        public CluckmasterCromwell(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(CluckToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my exclusive poultry shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Cluck Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CluckmasterCromwellQuest)
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

    public class SBCluckmasterCromwell : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBCluckmasterCromwell()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the bird-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(FireRooster), 1000, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(FrostHen), 1200, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(IllusionHen), 900, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(MysticFowl), 950, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(NecroRooster), 1100, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(PoisonPullet), 1300, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(ShadowChick), 1400, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(StoneRooster), 1250, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(Thunderbird), 1500, 10, 208, 0));
                Add(new AnimalBuyInfo(1, typeof(WindChicken), 1600, 10, 208, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class CluckToken : Item
    {
        [Constructable]
        public CluckToken() : base(0x1B72) // Change the Item ID to an appropriate value if necessary
        {
            Name = "Cluck Token";
        }

        public CluckToken(Serial serial) : base(serial)
        {
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
}
