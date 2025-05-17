using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TwelveStepsToRuinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Twelve Steps to Ruin"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Keir Steelhand*, the Golem Custodian of Castle British.\n\n" +
                    "His hands—fused with gleaming brass and worn steel—move with precise care as he tends to a collection of mechanical parts laid across a velvet-lined case.\n\n" +
                    "“Unit12 has gone rogue,” Keir states without emotion. “It transmits purge protocols to all maintenance constructs within Vault 44.”\n\n" +
                    "“I intercepted its control signal, dismantled its primary directive drive. But that only slowed it. Unit12 adapts. It learns.”\n\n" +
                    "“You must terminate it. Permanently. Before it corrupts the entire vault.”\n\n" +
                    "**Slay Unit12** before it initiates a full purge of the vault's golems. Return with confirmation, and I shall entrust you with something... delicate.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we risk losing control of Vault 44. I will keep trying, but alone, I fear it will be too late.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Unit12 still functions. Every moment we delay, the vault's systems decay further. Its influence spreads.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The vault breathes again... you’ve prevented a catastrophe. Take this: *VeilOfTheSilentThread*. Consider it a fragment of peace from the storm we nearly unleashed.";
            }
        }

        public TwelveStepsToRuinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Unit12), "Unit12", 1));
            AddReward(new BaseReward(typeof(VeilOfTheSilentThread), 1, "VeilOfTheSilentThread"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Twelve Steps to Ruin'!");
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

    public class KeirSteelhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TwelveStepsToRuinQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public KeirSteelhand()
            : base("the Golem Custodian", "Keir Steelhand")
        {
        }

        public KeirSteelhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 60);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 1001; // Pale skin with mechanical tint
            HairItemID = 0x203C; // Short-cropped
            HairHue = 1109; // Steel-gray
            FacialHairItemID = 0x2041; // Sculpted beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedHiroSode() { Hue = 2407, Name = "Machinist's Arms" }); // Brass-tone
            AddItem(new StuddedDo() { Hue = 2418, Name = "Custodian's Vestment" }); // Dark Iron
            AddItem(new StuddedLegs() { Hue = 2301, Name = "Greaves of Calibration" }); // Smoky Gray
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Tactile Servos" }); // Dark Brass
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Void-Touched Mantle" }); // Midnight Blue
            AddItem(new Boots() { Hue = 2207, Name = "Golem-Forge Boots" }); // Blackened Iron

            AddItem(new TacticalMultitool() { Hue = 2506, Name = "Custodian's Wrench" }); // Arcane Gold

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Custodian’s Pack";
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
