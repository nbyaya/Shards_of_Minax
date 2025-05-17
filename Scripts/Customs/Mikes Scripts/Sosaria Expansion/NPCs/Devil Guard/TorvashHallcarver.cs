using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WolvesInWallsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wolves in the Walls"; } }

        public override object Description
        {
            get
            {
                return
                    "Torvash Hallcarver, the grizzled carpenter of Devil Guard, rubs a hand roughened by years of timber work.\n\n" +
                    "\"It ain’t the rocks that stop me workin’—it’s them damn wolves. Nestin’ in the mine beams, gnawin’ at my supports, makin’ them claw marks. I've lost too many to their teeth already.\"\n\n" +
                    "\"I need that timber. Can’t shore up the mines if I’m watchin’ for fangs. Slay the MineWolf pack so I can work without lookin’ over my shoulder. Do that, and I’ll give you this **GreenwardensWreath**—a charm that’ll keep rot from touchin' your gear.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I suppose I’ll keep losin’ beams... and men. Wolves don’t wait for no one.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still not done? I can’t swing a hammer with wolves breathin’ down my neck.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? Good. I’ll sleep easier, and the mines’ll stand longer.\n\n" +
                       "Take this **GreenwardensWreath**—I made it from wood that still sings with life. It’ll serve you better than gold.";
            }
        }

        public WolvesInWallsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MineWolf), "MineWolf", 5)); // Assuming MineWolf is already defined
            AddReward(new BaseReward(typeof(GreenwardensWreath), 1, "GreenwardensWreath")); // Reward item defined elsewhere
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wolves in the Walls'!");
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

    public class TorvashHallcarver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WolvesInWallsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public TorvashHallcarver()
            : base("the Timberwright", "Torvash Hallcarver")
        {
        }

        public TorvashHallcarver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1108; // Tanned Skin
            HairItemID = 0x203C; // Short Hair
            HairHue = 1150; // Dark Brown
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2401, Name = "Timberguard Vest" }); // Dark wood hue
            AddItem(new LeatherLegs() { Hue = 2302, Name = "Carpenter's Breeches" }); // Muted brown
            AddItem(new LeatherGloves() { Hue = 2117, Name = "Wolfscarred Gloves" }); // Marked with claw scratches
            AddItem(new LeatherCap() { Hue = 2129, Name = "Dusty Workman's Cap" }); // Pale grey
            AddItem(new HalfApron() { Hue = 2101, Name = "Beam-Cutter's Apron" }); // Stained with sap
            AddItem(new Boots() { Hue = 1825, Name = "Ironshod Boots" }); // Reinforced for mine work

            AddItem(new CarpentersHammer() { Hue = 2407, Name = "Beamwright's Hammer" }); // Custom tool

            Backpack backpack = new Backpack();
            backpack.Hue = 2105;
            backpack.Name = "Timber Pack";
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
