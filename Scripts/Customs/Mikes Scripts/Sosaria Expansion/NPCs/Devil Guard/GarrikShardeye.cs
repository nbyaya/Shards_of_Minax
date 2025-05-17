using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElvenExterminateQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elven Exterminate"; } }

        public override object Description
        {
            get
            {
                return
                    "Garrik Shardeye, a renowned crystallographer, grips a jagged shard in his hand, his eyes gleaming with urgency.\n\n" +
                    "“I’ve devoted my life to the study of Sosaria’s living crystals, but the mines... they’ve become corrupted.”\n\n" +
                    "“A once-trusted guest, the ElfMiner, has gone mad—infecting the crystal veins with his rage and ruin. He shattered months of research... and my faith.”\n\n" +
                    "“Go into the Mines of Minax, track him, end him. If his madness spreads, we’ll lose more than ore—we’ll lose the very essence of these mountains.”\n\n" +
                    "**Slay the ElfMiner** and purify the mine’s spirit.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The crystals will continue to scream in silence. And we will drown in their fractured song.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The ElfMiner still taints the heart of the mine? Then our fate fractures by the hour.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the mines breathe easier. I can hear it in the crystals—they resonate true once more.\n\n" +
                       "Take this: *JudgementInAsh*. May its weight remind you of the balance you’ve restored.";
            }
        }

        public ElvenExterminateQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ElfMiner), "ElfMiner", 1));
            AddReward(new BaseReward(typeof(JudgementInAsh), 1, "JudgementInAsh"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elven Exterminate'!");
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

    public class GarrikShardeye : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElvenExterminateQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBStoneCrafter()); // Closest to his profession
        }

        [Constructable]
        public GarrikShardeye()
            : base("the Crystallographer", "Garrik Shardeye")
        {
        }

        public GarrikShardeye(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Frosty white
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2601, Name = "Crystalbound Tunic" }); // Pale shimmer-blue
            AddItem(new StuddedLegs() { Hue = 2613, Name = "Shardguard Leggings" });
            AddItem(new LeatherGloves() { Hue = 1152, Name = "Vein-Snare Gloves" });
            AddItem(new LeatherCap() { Hue = 2401, Name = "Splinterhelm" });
            AddItem(new HalfApron() { Hue = 2418, Name = "Miner's Wrap" });
            AddItem(new Boots() { Hue = 1810, Name = "Dustwalkers" });

            AddItem(new Scepter() { Hue = 1153, Name = "Shardeye's Focus" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Crystalist's Satchel";
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
