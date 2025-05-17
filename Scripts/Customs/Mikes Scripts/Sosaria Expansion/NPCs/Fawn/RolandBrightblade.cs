using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GnashingTheBeastQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gnashing the Beast"; } }

        public override object Description
        {
            get
            {
                return
                    "Roland Brightblade, a seasoned guard of Fawn, stands with a hardened gaze. His boots, adorned with *spiked iron cleats*, grind the dirt with every restless shift.\n\n" +
                    "“The ridge road isn’t safe anymore,” he growls. “That beast—**the Gnarrox**—has turned our patrols into meals. Bone-crushing jaws, eyes like coals in a dying fire. I’ve heard it at night, out there, gnashing, waiting.”\n\n" +
                    "“You want to help Fawn? Help me put that thing down. Before more families lose sons and daughters walking that cursed road.”\n\n" +
                    "**Kill the Gnarrox** before it tears through another patrol.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch your step out there. If the Gnarrox finds you first, you won’t have time to scream.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing, are you? Then the Gnarrox still feeds. We’re no safer than before.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You... you killed it? The road’s ours again. You’ve done Fawn a greater service than you know.\n\n" +
                       "Take this—**CelticLegendsChest**—earned by the blade, and by blood.";
            }
        }

        public GnashingTheBeastQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Gnarrox), "Gnarrox", 1));
            AddReward(new BaseReward(typeof(CelticLegendsChest), 1, "CelticLegendsChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Gnashing the Beast'!");
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

    public class RolandBrightblade : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GnashingTheBeastQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger()); // Closest profession for a town guard
        }

        [Constructable]
        public RolandBrightblade()
            : base("the Town Guard", "Roland Brightblade")
        {
        }

        public RolandBrightblade(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 85);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Raven-black
            FacialHairItemID = 0x203B; // Short beard
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 2101, Name = "Brightblade's Vigil" });
            AddItem(new RingmailChest() { Hue = 2406, Name = "Guard's Resolve" });
            AddItem(new StuddedLegs() { Hue = 2422, Name = "Spiked Iron Cleats" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Grasp of Duty" });
            AddItem(new Cloak() { Hue = 2117, Name = "Mantle of Fawn" });
            AddItem(new Broadsword() { Hue = 1150, Name = "Gnashbane" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guard's Kit";
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
