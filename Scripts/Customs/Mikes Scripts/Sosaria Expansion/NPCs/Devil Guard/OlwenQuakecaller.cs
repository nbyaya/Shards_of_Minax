using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SkeletonKeyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Skeleton Key"; } }

        public override object Description
        {
            get
            {
                return
                    "Olwen Quakecaller, mineralogist of Devil Guard, grips a jagged crystal rod, her gaze sharp with concern.\n\n" +
                    "“Something's wrong beneath the mines. My seismic sensors—tuned to the faintest tremors—light up like wildfire whenever that cursed MiningSkeleton swings its rusted pickaxe. The very earth cries out, you understand?”\n\n" +
                    "“That pick... it's not just steel and rust. It hums, resonates, shakes the stones like thunder. I fear it'll collapse the tunnels—or awaken something far worse beneath them.”\n\n" +
                    "**Slay the MiningSkeleton** and bring me its pickaxe. We must silence the tremors before Devil Guard crumbles into the abyss.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won't act, then pray the bones stay still, and the ground holds a little longer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it trembles... every moment, the mine edges closer to ruin. You must end this.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The tremors fade... I can feel it. The earth is calm—for now.\n\n" +
                       "This pickaxe, this relic of ruin... I will study it, see what secrets it holds.\n\n" +
                       "Take this, *Earthsplinter*. A shard of the earth’s fury, now yours to wield. May it serve you well, as you've served Devil Guard.";
            }
        }

        public SkeletonKeyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MiningSkeleton), "MiningSkeleton", 1));
            AddReward(new BaseReward(typeof(Earthsplinter), 1, "Earthsplinter"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Skeleton Key'!");
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

    public class OlwenQuakecaller : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SkeletonKeyQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public OlwenQuakecaller()
            : base("the Seismic Mineralogist", "Olwen Quakecaller")
        {
        }

        public OlwenQuakecaller(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale stone-gray complexion
            HairItemID = 0x2048; // Short hair
            HairHue = 1150; // Deep slate blue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2101, Name = "Quakecaller's Vest" }); // Deep earthy brown
            AddItem(new LeatherLegs() { Hue = 2418, Name = "Tremor-Ward Trousers" }); // Dust-grey
            AddItem(new LeatherGloves() { Hue = 2306, Name = "Stonegrip Gauntlets" }); // Mud-brown
            AddItem(new FeatheredHat() { Hue = 1107, Name = "Miner's Crest" }); // Dark mineral green
            AddItem(new HalfApron() { Hue = 1819, Name = "Crystal-Binder's Apron" }); // Deep sandstone
            AddItem(new FurBoots() { Hue = 1809, Name = "Seismic Stompers" }); // Earthy leather tone

            AddItem(new Pickaxe() { Hue = 2500, Name = "Vibrastone Pickaxe" }); // A mystical, resonant pickaxe

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Seismic Satchel";
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
