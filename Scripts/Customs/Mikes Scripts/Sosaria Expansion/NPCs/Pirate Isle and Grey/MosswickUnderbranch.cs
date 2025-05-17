using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DrakeInTheDepthsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drake in the Depths"; } }

        public override object Description
        {
            get
            {
                return
                    "Mosswick Underbranch, a curious herbalist garbed in sea-worn silks and strange baubles, eyes you with a blend of curiosity and desperation.\n\n" +
                    "“You hear it too, don’t you? The echoes beneath Exodus—*the cries of the damned*? That’s where the DoomedDrake roosts, where its petrified heart still beats.”\n\n" +
                    "“My draughts need its blood, tempered in shadow and time. Without it... sickness takes root. But none dare fetch it. Too many bones lie in those depths.”\n\n" +
                    "**Slay the DoomedDrake**, bring me its heart. Only then can the Isles be saved from creeping plague.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Plague or no, I’ll not force your hand. But know this—without the drake’s heart, death will take root in every corner of the Isles.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lives? The cries grow louder each night... I fear even the salt air cannot keep the sickness at bay much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The heart... it pulses still. Yes, yes, this will do.\n\n" +
                       "You’ve slain the drake and returned life to my craft. The Isles owe you.\n\n" +
                       "Take this *MemorialStone*, forged from the very earth the drake once haunted. May it mark your deeds in stone and memory.";
            }
        }

        public DrakeInTheDepthsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomedDrake), "DoomedDrake", 1));
            AddReward(new BaseReward(typeof(MemorialStone), 1, "MemorialStone"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Drake in the Depths'!");
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

    public class MosswickUnderbranch : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DrakeInTheDepthsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());
        }

        [Constructable]
        public MosswickUnderbranch()
            : base("the Driftwood Herbalist", "Mosswick Underbranch")
        {
        }

        public MosswickUnderbranch(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(65, 70, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 2115; // Mossy green
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1270, Name = "Salt-Worn Robe of the Tides" }); // Seafoam green
            AddItem(new BodySash() { Hue = 2210, Name = "Herbalist's Driftwood Sash" }); // Deep bark brown
            AddItem(new Sandals() { Hue = 2110, Name = "Coral-Laced Sandals" }); // Faded coral
            AddItem(new FeatheredHat() { Hue = 1194, Name = "Kelp-Plumed Hat" }); // Ocean blue
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Moss-Covered Gloves" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Satchel of Seaborne Herbs";
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
