using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GroundTheFlameBirdQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ground the FlameBird"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Iris Skybright*, a falconer known for her hawks that dance with the winds of West Montor.\n\n" +
                    "Clad in deep indigo and silvery plumage, her piercing eyes follow the horizon, searching.\n\n" +
                    "\"It came from the Gate of Hell... a **FlameBird**, wings of fire, trailing cinders like tears. It nearly took *Asha*, my finest hawk—her feathers still smolder with its touch.\"\n\n" +
                    "\"I've seen firebirds before. But this one, it burns for more than prey—it seeks to **ignite the roost**, the heart of our skies.\"\n\n" +
                    "\"I need you to bring it down. Before the flame spreads, before our skies belong to ash.\"\n\n" +
                    "**Shoot down the FlameBird** and save the falcon’s roost from ruin.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"If we let it soar, we lose not just our falcons, but the winds they command. Pray your heart stays light when skies grow dark.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"The FlameBird still soars? Then each gust carries death, and each night, I wonder if my hawks will return...\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"You brought it down? The sky sighs in relief—and so do I. *Asha* preens again, free from the burn.\"\n\n" +
                       "\"Take this: the **SabertoothSkull**. I found it long ago, beneath Moon’s sands—it’s yours now. Let it remind you that even the fiercest flames fall.\"";
            }
        }

        public GroundTheFlameBirdQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FlameBird), "the FlameBird", 1));
            AddReward(new BaseReward(typeof(SabertoothSkull), 1, "SabertoothSkull"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ground the FlameBird'!");
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

    public class IrisSkybright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GroundTheFlameBirdQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTanner()); // Closest to Falconer
        }

        [Constructable]
        public IrisSkybright()
            : base("the Falconer", "Iris Skybright")
        {
        }

        public IrisSkybright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 0x83F; // Pale sky-toned skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Skywoven Tunic" }); // Deep indigo
            AddItem(new LeatherSkirt() { Hue = 2101, Name = "Feathered Falconer's Wrap" }); // Soft slate grey
            AddItem(new LeatherGloves() { Hue = 1175, Name = "Talon-Grip Gloves" }); // Pale azure
            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of Winds" }); // Silver-blue shimmer
            AddItem(new FeatheredHat() { Hue = 2105, Name = "Aerie Plume" }); // Light dusk color
            AddItem(new Sandals() { Hue = 1170, Name = "Sky-Step Sandals" }); // Soft sky blue

            AddItem(new CompositeBow() { Hue = 1175, Name = "Starfletched Bow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Falconer's Pack";
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
