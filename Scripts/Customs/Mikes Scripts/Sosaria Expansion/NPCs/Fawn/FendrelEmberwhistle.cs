using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TrollfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Trollfall"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Fendrel Emberwhistle*, master blacksmith of Fawn, sweat gleaming on his brow as he hammers a twisted piece of metal back into form.\n\n" +
                    "He looks up, eyes burning with frustration and purpose.\n\n" +
                    "“Damn that Grothuuk... beast’s blocked the mountain pass, and my ore shipments’ve stopped dead. No ore, no blades. No blades, no defense for this town.”\n\n" +
                    "“They say the thing's hide shrugs off arrows, turns blades, and makes for a shield only the gods would break.”\n\n" +
                    "**“Slay Grothuuk. Free the pass. Bring peace—and the forge—back to life.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then Fawn will remain steel-starved. And the Grothuuk will feast on the bones of merchants.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The pass is still blocked? Bah! The forge grows colder by the day, and with it, our hope.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So Grothuuk’s no more? Ha! The clang of hammers will ring through Fawn once again!\n\n" +
                       "Here, take this: *BowOfAuriel*. Forged from the first ore I ever smelted, and blessed by the sun that lights our fair town. May it strike as true as your courage.";
            }
        }

        public TrollfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Grothuuk), "Grothuuk", 1));
            AddReward(new BaseReward(typeof(BowOfAuriel), 1, "BowOfAuriel"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Trollfall'!");
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

    public class FendrelEmberwhistle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TrollfallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public FendrelEmberwhistle()
            : base("the Forge-Master", "Fendrel Emberwhistle")
        {
        }

        public FendrelEmberwhistle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1157; // Charcoal Black
            FacialHairItemID = 0x204B; // Full Beard
            FacialHairHue = 1157;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2101, Name = "Ash-Forged Tunic" }); // Deep Charcoal
            AddItem(new StuddedLegs() { Hue = 2417, Name = "Molten Greaves" }); // Lava-Red
            AddItem(new LeatherGloves() { Hue = 2117, Name = "Hammer-Hardened Gloves" });
            AddItem(new LeatherCap() { Hue = 1820, Name = "Ember Helm" });
            AddItem(new HalfApron() { Hue = 1801, Name = "Forge-Touched Apron" });
            AddItem(new Boots() { Hue = 1813, Name = "Anvil-Stompers" });

            AddItem(new SmithSmasher() { Hue = 2301, Name = "Fendrel’s Iron Will" }); // Custom hammer
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
