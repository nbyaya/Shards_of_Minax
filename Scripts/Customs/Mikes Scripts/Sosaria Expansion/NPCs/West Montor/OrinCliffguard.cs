using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SmiteTheBurningGargoyleQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Smite the BurningGargoyle"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Orin Cliffguard*, a rugged stone mason with hands like granite and eyes sharp as chisels.\n\n" +
                    "His clothes are covered in fine stone dust, and beside him lies a partially carved statue—its face warped by recent cracks.\n\n" +
                    "\"Every block I set splits. Every carving I make, it crumbles. And all because that blasted **BurningGargoyle** has taken to roost above my newest works!\"\n\n" +
                    "\"I studied under the finest minds in Britain—**Elena the Archivist** herself! I won't have my life's craft undone by some fiery pest!\"\n\n" +
                    "\"The beast perches near the **Gate of Hell**, where flame and stone fight for dominance. It watches. Waits. Laughs, I swear. Its heat alone warps the air, twists my work.\"\n\n" +
                    "\"Will you drive it back to whatever infernal forge birthed it? Do this, and I’ll see you rewarded—not just in gold, but with a relic said to fan away evil itself. **The ZhugeFeathersFan.**\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the stones bear the scars of cowardice. I will not stop my craft—but my work will weep until the gargoyle is slain.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still clings to the cliffs? Still mocks me from the heights? Strike it down, before the Gate of Hell itself collapses upon us all!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone? Truly? Then the mountain breathes easier—and so do I.\n\n" +
                       "Thank you, friend. You've not just slain a beast—you’ve preserved the soul of **West Montor’s stone**.\n\n" +
                       "Take this: **ZhugeFeathersFan**. May it cool your path and sweep away whatever evils try to cling.";
            }
        }

        public SmiteTheBurningGargoyleQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BurningGargoyle), "BurningGargoyle", 1));
            AddReward(new BaseReward(typeof(ZhugeFeathersFan), 1, "ZhugeFeathersFan"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Smite the BurningGargoyle'!");
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

    public class OrinCliffguard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SmiteTheBurningGargoyleQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBStoneCrafter());
        }

        [Constructable]
        public OrinCliffguard()
            : base("the Stone Mason", "Orin Cliffguard")
        {
        }

        public OrinCliffguard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered Tan
            HairItemID = 0x203C; // Short Hair
            HairHue = 1813; // Ashen Brown
            FacialHairItemID = 0x204B; // Full Beard
            FacialHairHue = 1813;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 1820, Name = "Mason’s Mantle" }); // Dark Slate
            AddItem(new StuddedLegs() { Hue = 1812, Name = "Cliffguard Trousers" }); // Dusty Gray
            AddItem(new LeatherGloves() { Hue = 2406, Name = "Stonecutter’s Grips" }); // Pale Granite
            AddItem(new LeatherCap() { Hue = 2101, Name = "Mason’s Cap" }); // Earth Brown
            AddItem(new HalfApron() { Hue = 1811, Name = "Chisel-Bitten Apron" }); // Soot Black
            AddItem(new FurBoots() { Hue = 1819, Name = "Carver's Boots" }); // Deep Slate

            AddItem(new HammerPick() { Hue = 2500, Name = "Masterwork Chisel" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Stoneworker’s Satchel";
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
