using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LadenWithDreadQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Laden with Dread"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Baron Tradeborne*, Dawn’s most prominent merchant, seated beneath a silk canopy in the town’s bustling market.\n\n" +
                    "He adjusts his crimson cloak, heavy with golden embroidery, but there’s a pall over his usually shrewd eyes.\n\n" +
                    "“Dawn thrives on trade... and trade demands roads free of dread.”\n\n" +
                    "“A creature stalks the eastern pass—**a Doomridden Packhorse**, skeletal and foul, laden with crates from my missing caravans. It tramples merchants, groaning under cursed cargo. My wagons halt. My wealth bleeds.”\n\n" +
                    "“I need more than swords. I need resolve. **Slay this beast**, free my wares, and lift this dread from Dawn.”\n\n" +
                    "“Return with proof of its death... and I’ll make sure the sun shines gold upon your path.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to watch my trade rot, and my people despair.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still stomps? My ledgers grow heavy with ruin. Slay it—before it becomes legend.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The road breathes again, and my wares shall move.\n\n" +
                       "Take this: **SunkenThunder**. Once lost in the depths of Doom, now yours. May it strike true against shadows yet to come.";
            }
        }

        public LadenWithDreadQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomriddenPackhorse), "Doomridden Packhorse", 1));
            AddReward(new BaseReward(typeof(SunkenThunder), 1, "SunkenThunder"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Laden with Dread'!");
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

    public class BaronTradeborne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(LadenWithDreadQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public BaronTradeborne()
            : base("the Merchant of Dawn", "Baron Tradeborne")
        {
        }

        public BaronTradeborne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1154; // Jet Black
            FacialHairItemID = 0x204B; // Trimmed beard
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Silken Trader's Shirt" }); // Deep red
            AddItem(new LongPants() { Hue = 1157, Name = "Golden-Threaded Slacks" }); // Gold-tinged
            AddItem(new BodySash() { Hue = 1151, Name = "Baron's Sash of Trade" }); // Rich blue
            AddItem(new Cloak() { Hue = 1154, Name = "Velvet Cloak of the Eastern Winds" }); // Black
            AddItem(new Boots() { Hue = 1150, Name = "Roadworn Merchant's Boots" }); // Dusty grey
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Tradeborne's Crimson Plume" }); // Crimson with plume

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Merchant's Blue
            backpack.Name = "Ledger Satchel";
            AddItem(backpack);
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
}
