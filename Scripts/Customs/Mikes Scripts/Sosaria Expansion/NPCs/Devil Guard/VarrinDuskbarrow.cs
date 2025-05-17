using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HauntingHarvestQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Haunting Harvest"; } }

        public override object Description
        {
            get
            {
                return
                    "Varrin Duskbarrow, cloaked in grave-dark hues, his fingers smudged with ash, speaks in a low voice heavy with regret.\n\n" +
                    "“The Mines of Minax... I thought to honor the dead. To mark their rest with runes of peace. But I dug too deep, unearthed what should not stir.”\n\n" +
                    "“A GhoulMiner now haunts the shaft, bound to veins of Ghastly Ore I disturbed. His hammer strikes not for gold, but for souls. Each blow echoes with suffering, trapping spirits beneath the earth.”\n\n" +
                    "“Free them. Slay the GhoulMiner. Let them rest once more.”\n\n" +
                    "**Bring silence to the Mines. Destroy the GhoulMiner and end the haunting harvest.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the dead will cry alone, and the ore will weep blood in silence.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The GhoulMiner still labors? His pick gnaws deeper, and the dead grow restless.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The echoes have ceased... the tombs lie still. You’ve done a kindness few would understand.\n\n" +
                       "**Take these WyrmhideBreeches**, wrought to endure cursed soil. May they carry you safely through darker paths.";
            }
        }

        public HauntingHarvestQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GhoulMiner), "GhoulMiner", 1));
            AddReward(new BaseReward(typeof(WyrmhideBreeches), 1, "WyrmhideBreeches"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Haunting Harvest'!");
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

    public class VarrinDuskbarrow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HauntingHarvestQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); // Closest vendor type for a Tomb Keeper / Seer
        }

        [Constructable]
        public VarrinDuskbarrow()
            : base("the Tomb Keeper", "Varrin Duskbarrow")
        {
        }

        public VarrinDuskbarrow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 45);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1025; // Pale
            HairItemID = 0x2049; // Long hair
            HairHue = 1109; // Dusty black
            FacialHairItemID = 0x2041; // Medium beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1108, Name = "Gravekeeper's Shroud" }); // Deep grey-black
            AddItem(new HoodedShroudOfShadows() { Hue = 1109, Name = "Tombveil Hood" });
            AddItem(new Sandals() { Hue = 1150, Name = "Silent Step Sandals" });

            AddItem(new Scepter() { Hue = 1153, Name = "Rune-etched Staff" }); // A scepter with grave runes

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bone-Pouch";
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
