using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScaleAndShatterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scale and Shatter"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Grelda Stonearm*, Mining Guildmaster of Mountain Crest.\n\n" +
                    "Her stance is unyielding, like the stone beneath her feet, and her arms bear the scars of a lifetime in the mines.\n\n" +
                    "\"My family carved these tunnels with their bare hands. Now some *Coldscale* bastard thinks it can prey on my miners?\"\n\n" +
                    "\"It’s hiding deep in the Ice Cavern, just beyond the lower veins where the cold bites hardest. \n\n" +
                    "**Slay the Coldscale Lizardman** that ambushes my folk, and you'll have not just my thanks, but a gift forged for those who walk the veins without fear.\"\n\n" +
                    "There's talk of a hidden cache down there too—ice-etched ore that the beasts are guarding.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"I’ll not force your hand, but mark my words—the mines won’t wait long for heroes to act.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"The Coldscale still prowls? Every day it lives, more blood stains our stones. Get back out there.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"So, the beast is dead? You’ve done more than protect lives—you’ve kept our pride intact. \n\n" +
                       "Take this, *HarvestersStride*. May it carry you through many more dangers beneath the stone and sky.\"";
            }
        }

        public ScaleAndShatterQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ColdscaleLizardman), "Coldscale Lizardman", 1));
            AddReward(new BaseReward(typeof(HarvestersStride), 1, "HarvestersStride"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scale and Shatter'!");
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

    public class GreldaStonearm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScaleAndShatterQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public GreldaStonearm()
            : base("the Mining Guildmaster", "Grelda Stonearm")
        {
        }

        public GreldaStonearm(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1023; // Weathered skin tone
            HairItemID = 0x203D; // Braided Hair
            HairHue = 1107; // Slate-grey
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedBustierArms() { Hue = 2424, Name = "Frostvein Vest" }); // Frost-toned leather
            AddItem(new LeatherLegs() { Hue = 1819, Name = "Stonewalker Breeches" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Ore-Grip Gloves" });
            AddItem(new LeatherCap() { Hue = 1820, Name = "Miner's Mantle" });
            AddItem(new HalfApron() { Hue = 1815, Name = "Guildmaster's Apron" });
            AddItem(new FurBoots() { Hue = 1108, Name = "Deepdelver Boots" });

            AddItem(new Pickaxe() { Hue = 2505, Name = "Stonearm's Pick" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Guildmaster's Pack";
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
