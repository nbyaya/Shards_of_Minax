using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BeatrixQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Great Llama Cull";

        public override object Description => 
            "Greetings, adventurer. I am Beatrix of the Quiet Steppes.\n\n" +
            "My fields have been overrun by... llamas. Don't let their wooly exteriors fool you. " +
            "They spit. They stomp. They eat *everything*.\n\n" +
            "I need a bold soul to cull five hundred of the beasts. Do this, and I will grant you access to rare tamed llamas... " +
            "and other goods salvaged from their past victims.\n\n" +
            "Will you be my exterminator?";

        public override object Refuse => "A shame. The llamas shall overrun us all.";

        public override object Uncomplete => "You’ve not yet cleared enough of the menace. Return when 500 llamas lie vanquished.";

        public override object Complete =>
            "Bless you, llama-slayer! The fields are safe once more... for now.\n\n" +
            "My shop is now open to you. Choose wisely from my wares, and may no llama ever spit in your face again.";

        public BeatrixQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Llama), "Llamas", 500));
            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.LlamaSlayerQuest))
                profile.Talents[TalentID.LlamaSlayerQuest] = new Talent(TalentID.LlamaSlayerQuest);

            profile.Talents[TalentID.LlamaSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have culled 500 llamas for Beatrix!");
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
            reader.ReadInt();
        }
    }

    public class Beatrix : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeatrix());
        }

        [Constructable]
        public Beatrix() : base("Beatrix", "Llama Evictor")
        {
        }

        public Beatrix(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049);
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Skirt(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new Sandals());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.LlamaSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You’ve earned your reward, llama-hunter. Have a look.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The llamas still roam free. Return when 500 are dust.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(BeatrixQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SBBeatrix : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Llama), 5000, 10, 0xDC, 0));
                Add(new GenericBuyInfo(typeof(FairyOil), 5000, 10, 0xF0E, 0));
                Add(new GenericBuyInfo(typeof(FleshLight), 5000, 10, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(EssentialBooks), 5000, 10, 0xFF1, 0));
                Add(new GenericBuyInfo(typeof(PileOfChains), 5000, 10, 0x1B3F, 0));
                Add(new GenericBuyInfo(typeof(FancyCopperSunflower), 5000, 10, 0xC95, 0));
                Add(new GenericBuyInfo(typeof(FineHoochJug), 5000, 10, 0x9C8, 0));
                Add(new GenericBuyInfo(typeof(SpookyGhost), 5000, 10, 0x1F0B, 0));
                Add(new GenericBuyInfo(typeof(ScentedCandle), 5000, 10, 0xA26, 0));
                Add(new GenericBuyInfo(typeof(SnowSculpture), 5000, 10, 0x232A, 0));
                Add(new GenericBuyInfo(typeof(MasterShrubbery), 5000, 10, 0xCEA, 0));
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
