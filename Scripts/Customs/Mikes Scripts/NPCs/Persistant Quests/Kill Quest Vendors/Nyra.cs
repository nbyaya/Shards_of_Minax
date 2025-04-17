using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class NyraQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Hunt of 500 Panthers";

        public override object Description =>
            "I am Nyra, once a whisper in the jungle, now a shadow of vengeance.\n\n" +
            "The panthers have grown wild, feral beyond reason. Once protectors of balance, now they hunt without purpose.\n\n" +
            "Slay *five hundred* panthers to restore harmony. Only then shall I offer you the relics of the deep jungle.";

        public override object Refuse => "Then leave this place. Harmony cannot wait on hesitant hearts.";

        public override object Uncomplete => "You have not yet slain enough. The panthers still reign free.";

        public override object Complete =>
            "You return... and the jungle breathes easier.\n\n" +
            "The spirits are calmed. You have done what few dared.\n\n" +
            "My shop is now yours to browse. May what you find serve you well.";

        public NyraQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Panther), "Panthers", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.PantherSlayerQuest))
                profile.Talents[TalentID.PantherSlayerQuest] = new Talent(TalentID.PantherSlayerQuest);
            
            profile.Talents[TalentID.PantherSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Panthers for Nyra!");
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

    public class Nyra : MondainQuester
    {
        public override bool IsActiveVendor => true;
        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNyra());
        }

        [Constructable]
        public Nyra() : base("Nyra", "the Silent Huntress")
        {
        }

        public Nyra(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Elf;
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049;
            HairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new LeatherBustierArms() { Hue = 2101 });
            AddItem(new LeatherSkirt() { Hue = 2101 });
            AddItem(new Sandals(1175));
            AddItem(new Cloak(1175));
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.PantherSlayerQuest, out var talent) && talent.Points > 0)
                {
                    SayTo(from, "You have proven your worth. Browse freely.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "The jungle still calls for blood. Return when you have slain 500 Panthers.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(NyraQuest) };

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

    public class SBNyra : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Panther), 5000, 10, 0xD6, 0));
                Add(new GenericBuyInfo(typeof(MemorialCopper), 5000, 20, 0x1BE9, 0));
                Add(new GenericBuyInfo(typeof(LargeVat), 5000, 20, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(YardCupid), 5000, 20, 0x2BDD, 0));
                Add(new GenericBuyInfo(typeof(FieldPlow), 5000, 20, 0x15FB, 0));
                Add(new GenericBuyInfo(typeof(ChaliceOfPilfering), 5000, 20, 0x2F5D, 0));
                Add(new GenericBuyInfo(typeof(Jet), 5000, 20, 0xF21, 0));
                Add(new GenericBuyInfo(typeof(InfinitySymbol), 5000, 20, 0x2BEE, 0));
                Add(new GenericBuyInfo(typeof(AncientWood), 5000, 20, 0x1BD7, 0));
                Add(new GenericBuyInfo(typeof(PopStarsTrove), 5000, 20, 0x2CFF, 0));
                Add(new GenericBuyInfo(typeof(RadBoomboxTrove), 5000, 20, 0x2D00, 0));
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
