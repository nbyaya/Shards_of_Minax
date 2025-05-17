using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MarrowsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Marrow's End"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Tristane Bonecarver*, Mountain Crest's famed sculptor of bone.\n\n" +
                    "His workshop is lined with delicate, ethereal sculptures—crafted not just from ivory, but from bones carefully selected from the land.\n\n" +
                    "His eyes, pale as moonlight, flicker with restrained fury as he gestures toward the icy peaks beyond.\n\n" +
                    "“They mock my work—those abominations in the cavern. I carve life’s memory from bone, but the Frozen Bonefiend... it perverts marrow into a vessel for dread.”\n\n" +
                    "“I won’t stand for it. That fiend animates the bones of the dead, filling empty sockets with a light that should never burn. **Marrow should rest. Bones should be honored, not enslaved.**”\n\n" +
                    "“Go into the Ice Cavern. Shatter that monstrosity. Let no bone rise under its sway again.”\n\n" +
                    "**Slay the Frozen Bonefiend** and return. Let me craft peace from its remains.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to mourn in silence, for every day that thing lives, it mocks all I’ve ever shaped.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it clatters in the dark? My hands tremble—I cannot carve while it exists. Bones sing of torment.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The marrow sleeps again?\n\n" +
                       "You’ve freed the bones from that unholy flame. **Take this circlet**—forged in courtly style, but lined with runes of remembrance.\n\n" +
                       "Let it guard your mind, as you have guarded the dead’s rest.";
            }
        }

        public MarrowsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrozenBonefiend), "Frozen Bonefiend", 1));
            AddReward(new BaseReward(typeof(CourtiersRegalCirclet), 1, "CourtiersRegalCirclet"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Marrow's End'!");
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

    public class TristaneBonecarver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MarrowsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); // Closest vendor type for a bone sculptor
        }

        [Constructable]
        public TristaneBonecarver()
            : base("the Bone Craftsman", "Tristane Bonecarver")
        {
        }

        public TristaneBonecarver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 45);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Ash-white
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new BoneHelm() { Hue = 2101, Name = "Carver’s Crest" }); // Pale bone hue
            AddItem(new StuddedChest() { Hue = 2105, Name = "Marrowguard Vest" });
            AddItem(new StuddedLegs() { Hue = 2105, Name = "Bonebinder Greaves" });
            AddItem(new StuddedGloves() { Hue = 2101, Name = "Ivorygrip Gloves" });
            AddItem(new Cloak() { Hue = 2412, Name = "Shroud of Silent Remains" });
            AddItem(new Boots() { Hue = 2401, Name = "Stiller’s Tread" });

            AddItem(new BoneHarvester() { Hue = 2500, Name = "Marrow Cleaver" }); // Custom bone-themed weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Carver’s Pack";
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
