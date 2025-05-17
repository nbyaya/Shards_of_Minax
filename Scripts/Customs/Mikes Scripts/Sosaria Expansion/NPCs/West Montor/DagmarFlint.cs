using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CrushFirestoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crush the Firestone"; } }

        public override object Description
        {
            get
            {
                return
                    "Dagmar Flint, Forewoman of the West Montor mines, glares at you through soot-smeared goggles.\n\n" +
                    "“The mines are failing. Shafts collapsing, ore veins gone brittle. It’s the heat, damn it—heat like the gods are stoking their forge beneath our feet.”\n\n" +
                    "“We found glyphs, charred into the rock. Warnings, maybe. Curses, more likely. My best pickman swears he saw something—**a beast of molten stone**. It walks the lower shafts, melting the walls with every step.”\n\n" +
                    "**“FirestoneElemental.** That’s what they call it now. But I just call it trouble.”\n\n" +
                    "“Kill it. Smash it to slag before the whole mine comes down on us.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then you best not come near my mines again. I won’t have cowards around while my people burn.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You still here? My shafts are buckling, and that fiery brute still roams. Do your job, or we’re finished.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it... I can feel it. The earth’s still hot, but it’s not angry anymore.\n\n" +
                       "Take this, a tunic passed down from the old warriors of Ulster. Worn by those who don’t flinch at fire or fear.\n\n" +
                       "You’re no miner, but today, you’ve earned your grit.";
            }
        }

        public CrushFirestoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FirestoneElemental), "FirestoneElemental", 1));
            AddReward(new BaseReward(typeof(WarriorOfUlstersTunic), 1, "Warrior of Ulster’s Tunic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Crush the Firestone'!");
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

    public class DagmarFlint : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CrushFirestoneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel());
        }

        [Constructable]
        public DagmarFlint()
            : base("the Miner’s Forewoman", "Dagmar Flint")
        {
        }

        public DagmarFlint(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 75);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 2002; // Weathered, tanned
            HairItemID = 8251; // Short, tied back
            HairHue = 1810; // Ash-black
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2425, Name = "Fireproofed Leather Tunic" }); // Charred-iron gray
            AddItem(new StuddedLegs() { Hue = 2955, Name = "Veinwalker Greaves" }); // Burnt copper
            AddItem(new LeatherGloves() { Hue = 2302, Name = "Ore-Grip Gloves" });
            AddItem(new LeatherCap() { Hue = 1820, Name = "Flint-Hardened Helm" });
            AddItem(new HalfApron() { Hue = 1841, Name = "Forewoman’s Utility Apron" });
            AddItem(new FurBoots() { Hue = 1815, Name = "Ashen Miner's Boots" });

            AddItem(new Pickaxe() { Hue = 2950, Name = "Embertooth Pickaxe" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Miner's Gear Pack";
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
