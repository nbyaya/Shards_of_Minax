using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LlamaLeclairQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Llama Leclair's Llama Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Llama Leclair, the Great Llama Enthusiast. " +
                    "To prove your worth and gain access to my exclusive llama shop, you must " +
                    "show your skill in defeating the rarest and most peculiar llamas in the land. " +
                    "Complete these tasks:\n\n" +
                    "1. Slay a Cactus Llama.\n" +
                    "2. Defeat a Charro Llama.\n" +
                    "3. Conquer a Dia De Los Muertos Llama.\n" +
                    "4. Overcome an El Mariachi Llama.\n" +
                    "5. Destroy a Fiesta Llama.\n" +
                    "6. Vanquish a Luchador Llama.\n" +
                    "7. Subdue a Sombrero De Sol Llama.\n" +
                    "8. Eradicate a Sombrero Llama.\n" +
                    "9. Defeat a Taco Llama.\n" +
                    "10. Exterminate a Tequila Llama.\n\n" +
                    "Complete these tasks, and I will grant you access to my llama-themed shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to embrace the llama challenge."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. The llamas await your challenge!"; } }

        public override object Complete { get { return "Incredible! You have proven yourself against the greatest llamas. My shop is now open to you!"; } }

        public LlamaLeclairQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CactusLlama), "Cactus Llamas", 1));
            AddObjective(new SlayObjective(typeof(CharroLlama), "Charro Llamas", 1));
            AddObjective(new SlayObjective(typeof(DiaDeLosMuertosLlama), "Dia De Los Muertos Llamas", 1));
            AddObjective(new SlayObjective(typeof(ElMariachiLlama), "El Mariachi Llamas", 1));
            AddObjective(new SlayObjective(typeof(FiestaLlama), "Fiesta Llamas", 1));
            AddObjective(new SlayObjective(typeof(LuchadorLlama), "Luchador Llamas", 1));
            AddObjective(new SlayObjective(typeof(SombreroDeSolLlama), "Sombrero De Sol Llamas", 1));
            AddObjective(new SlayObjective(typeof(SombreroLlama), "Sombrero Llamas", 1));
            AddObjective(new SlayObjective(typeof(TacoLlama), "Taco Llamas", 1));
            AddObjective(new SlayObjective(typeof(TequilaLlama), "Tequila Llamas", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(LlamaToken), 1, "Llama Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have conquered the llama challenge!");
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

    public class LlamaLeclair : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLlamaLeclair());
        }

        [Constructable]
        public LlamaLeclair()
            : base("Llama Leclair", "The Great Llama Enthusiast")
        {
        }

        public LlamaLeclair(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(LlamaToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my exclusive llama shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Llama Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LlamaLeclairQuest)
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

    public class SBLlamaLeclair : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBLlamaLeclair()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the llama-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(CactusLlama), 1000, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(CharroLlama), 1200, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(DiaDeLosMuertosLlama), 950, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(ElMariachiLlama), 1100, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(FiestaLlama), 1050, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(LuchadorLlama), 1300, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SombreroDeSolLlama), 1400, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(SombreroLlama), 1250, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TacoLlama), 1150, 10, 13, 0));
                Add(new AnimalBuyInfo(1, typeof(TequilaLlama), 1200, 10, 13, 0));
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
