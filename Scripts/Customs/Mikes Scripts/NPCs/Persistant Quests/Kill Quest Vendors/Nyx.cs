using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class NyxQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Nightmare's End";

        public override object Description => 
            "I am Nyx, Seeker of Shadows. A plague rides the darkness—Nightmares, creatures born of fear and fire, " +
            "run rampant through our realm.\n\n" +
            "Their howls chill the soul, their hooves crack stone. Bring me the death of *five hundred* such beasts, " +
            "and I shall reward you with access to rare, arcane curiosities and the right to purchase a tamed Nightmare.\n\n" +
            "When the last of their screams dies on the wind, return to me.";

        public override object Refuse => "Then begone. I shall not suffer dreamers who fear the dark.";

        public override object Uncomplete => "The Nightmares still run wild. Do not return until their shadows are thinned.";

        public override object Complete => 
            "Your eyes gleam with the fire of victory. The Nightmares are no more—your hand has delivered retribution.\n\n" +
            "My wares are now open to you: talismans of horror, tools of shadow, and companions drawn from fear itself.\n\n" +
            "Use them wisely, or not at all.";

        public NyxQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Nightmare), "Nightmares", 500));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.NightmareSlayerQuest))
                profile.Talents[TalentID.NightmareSlayerQuest] = new Talent(TalentID.NightmareSlayerQuest);

            profile.Talents[TalentID.NightmareSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Nightmares for Nyx!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class Nyx : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNyx());
        }

        [Constructable]
        public Nyx() : base("Nyx", "Nightmare Hunter")
        {
        }

        public Nyx(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049; // Long hair
            HairHue = 1175; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x497)); // Deep violet robe
            AddItem(new Sandals(0x455));
            AddItem(new Cloak(0x455));
            AddItem(new Backpack());
        }

        public override void VendorBuy(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.NightmareSlayerQuest, out Talent talent) && talent.Points > 0)
                {
                    SayTo(from, "You’ve proven yourself worthy. Browse my wares.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You are not ready. Return when the Nightmares are dust.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(NyxQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class SBNyx : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(Nightmare), 5000, 10, 17, 0)); // Tamed Nightmare

                // All items cost 5000gp
                Add(new GenericBuyInfo(typeof(GraniteHammer), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(StuffedDoll), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(FancyMirror), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(ExoticPlum), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(Shears), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(FillerPowder), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(HorrorPumpkin), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(HumanCarvingKit), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(HangingMask), 5000, 10, 0, 0));
                Add(new GenericBuyInfo(typeof(SkullIncense), 5000, 10, 0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
