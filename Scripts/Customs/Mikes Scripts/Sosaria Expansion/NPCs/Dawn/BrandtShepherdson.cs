using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RottenRemainsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rotten Remains"; } }

        public override object Description
        {
            get
            {
                return
                    "Brandt Shepherdson clutches his crook tightly, his face lined with worry and dirt from the fields.\n\n" +
                    "\"The sheep... they’re not right. First, it was the rot, then the coughing, and now—now they see something I can’t. Wolves dragged bones from the old Doom dungeon, and ever since, a sick mist chokes the pens.\"\n\n" +
                    "**The Corpse of Doom** rises near his land, spreading pestilence wherever it treads. Brandt’s voice trembles with both fear and anger.\n\n" +
                    "\"You’ve got to stop it. Kill it, burn it, whatever it takes—just save my flock. Save Dawn.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Brandt nods grimly. \"I’ll keep watch, but I fear each night brings more death.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"It still walks? The grass dies faster now. Even the dogs won’t go near.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"You’ve done it... The rot's lifting. I can breathe again. So can they.\"\n\n" +
                       "He pulls a pair of gauntlets from an old, stained sack. \"These are Razorroot Gauntlets—made for cutting through thorns and worse. May they serve you as well as you’ve served me.\"";
            }
        }

        public RottenRemainsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorpseOfDoom), "Corpse of Doom", 1));
            AddReward(new BaseReward(typeof(RazorrootGauntlets), 1, "Razorroot Gauntlets"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rotten Remains'!");
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

    public class BrandtShepherdson : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RottenRemainsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRancher());
        }

        [Constructable]
        public BrandtShepherdson()
            : base("the Shepherd", "Brandt Shepherdson")
        {
        }

        public BrandtShepherdson(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1813; // Weathered brown
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1813;
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 1846, Name = "Shepherd’s Workrobe" }); // Earthy green
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Rot-Touched Gloves" }); // Dull black
            AddItem(new Cloak() { Hue = 1825, Name = "Mist-Worn Cloak" }); // Faded grey
            AddItem(new Boots() { Hue = 1109, Name = "Flock-Keeper’s Boots" }); // Muddy brown
            AddItem(new WideBrimHat() { Hue = 1845, Name = "Sunshield Hat" }); // Straw gold
            AddItem(new ShepherdsCrook() { Hue = 2101, Name = "Brandt’s Crook" }); // Weathered oak

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Shepherd’s Pack";
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
