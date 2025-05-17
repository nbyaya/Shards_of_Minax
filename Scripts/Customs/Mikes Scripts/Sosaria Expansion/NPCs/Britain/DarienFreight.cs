using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SupplyChainBreakerQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Supply Chain Breaker"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Darien Freight*, Logistics Master of Castle British. His coat is marked with ink stains, and a satchel of tracking glyphs hangs at his side.\n\n" +
                    "His voice is curt, clipped, as he rifles through damaged ledger books.\n\n" +
                    "“Our provisions. Gone. Crates devoured in the night, pallets dragged into the depths.”\n\n" +
                    "“I’ve seen it. **The SupplyBeast**. A creature twisted by Preservation Vault 44’s foul anomalies. It hoards what it doesn’t eat, mocks us with our own stores.”\n\n" +
                    "“I’ve tagged the remaining crates with glyphs, but the beast moves fast. We can’t afford to lose more—not when winter looms.”\n\n" +
                    "**Hunt down the SupplyBeast in Vault 44 and restore our supply chain.** Or we’ll be starving by moon’s end.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then stay out of my storerooms. I’ve no use for onlookers when crates vanish and people go hungry.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still loose? Then the vault still devours us. Every hour lost, more goods are gone.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s dead? Good. Perhaps now, we can breathe.\n\n" +
                       "Take this: *Navis Requiem*. A token, but more—a reminder. That even beasts can’t break our chain.";
            }
        }

        public SupplyChainBreakerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SupplyBeastV44), "SupplyBeast", 1));
            AddReward(new BaseReward(typeof(NavisRequiem), 1, "Navis Requiem"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Supply Chain Breaker'!");
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

    public class DarienFreight : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SupplyChainBreakerQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public DarienFreight()
            : base("the Logistics Master", "Darien Freight")
        {
        }

        public DarienFreight(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Faded steel-gray
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet() { Hue = 1157, Name = "Logistics Tunic of Slate" }); // Slate-gray
            AddItem(new LongPants() { Hue = 1109, Name = "Dust-Stained Trousers" });
            AddItem(new HalfApron() { Hue = 1820, Name = "Marked Ledger Apron" });
            AddItem(new Boots() { Hue = 2101, Name = "Warehouseman's Boots" });
            AddItem(new Cloak() { Hue = 1175, Name = "Vault-Touched Cloak" }); // Faint shimmer
            AddItem(new SkullCap() { Hue = 1102, Name = "Countermaster’s Cap" });

            AddItem(new Spellbook() { Hue = 0, Name = "Crate Inventory Log" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Tagged Crate Satchel";
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
