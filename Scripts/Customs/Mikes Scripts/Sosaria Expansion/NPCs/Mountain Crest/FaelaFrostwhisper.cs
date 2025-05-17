using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SatyrsFrozenRevelQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Satyr’s Frozen Revel"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Faela Frostwhisper*, Forest Warden of Mountain Crest.\n\n" +
                    "She stands vigilant beneath frost-kissed boughs, clad in hues of snow and forest twilight. Her voice is calm but grave.\n\n" +
                    "\"These woods are sick. Ice beasts roam where deer once grazed. The cause? A *Chill Satyr*, corrupted by wild, wintery magic. His cursed revels draw creatures of frost into our realm.\"\n\n" +
                    "\"I’ve held these woods for decades, but nature's rhythm is twisted now. If the Satyr remains, even the heartwood may freeze.\"\n\n" +
                    "**Slay the Chill Satyr** haunting the Ice Cavern and restore the balance before winter becomes eternal.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I must continue alone, though I fear the frost will soon take root where hope once bloomed.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Satyr still dances? His laughter chills my bones. We must end this revel, or our woods will wither.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The ice wanes, and the woods breathe anew.\n\n" +
                       "You have done more than slay a beast—you've returned *life* to the forest.\n\n" +
                       "Take these *Exotic Boots*—crafted for paths both wild and cold. May they carry you to places untouched by frost or fear.";
            }
        }

        public SatyrsFrozenRevelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ChillSatyr), "Chill Satyr", 1));
            AddReward(new BaseReward(typeof(ExoticBoots), 1, "Exotic Boots"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Satyr’s Frozen Revel'!");
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

    public class FaelaFrostwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SatyrsFrozenRevelQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public FaelaFrostwhisper()
            : base("the Forest Warden", "Faela Frostwhisper")
        {
        }

        public FaelaFrostwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Elf;

            Hue = 1150; // Pale, frost-kissed skin
            HairItemID = 0x2048; // Long Hair
            HairHue = 1153; // Icy blue
        }

        public override void InitOutfit()
        {
            AddItem(new WoodlandBelt() { Hue = 2052, Name = "Frostbark Sash" });
            AddItem(new WoodlandChest() { Hue = 2101, Name = "Glacierwoven Vest" });
            AddItem(new WoodlandArms() { Hue = 2063, Name = "Ivybound Sleeves" });
            AddItem(new WoodlandLegs() { Hue = 2063, Name = "Snowhide Trousers" });
            AddItem(new FurBoots() { Hue = 2105, Name = "Winterbound Boots" });
            AddItem(new FeatheredHat() { Hue = 2101, Name = "Frostfeather Cap" });
            AddItem(new GnarledStaff() { Hue = 2106, Name = "Staff of the Silent Grove" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Warden's Pack";
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
