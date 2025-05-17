using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BrigandsBaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Brigand’s Bane"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Nisha Moonson*, Renika’s renowned Cartographer. Her fingers trace faded ink on a half-finished map, eyes scanning the horizon with wary resolve.\n\n" +
                    "“Every cove, every inlet—I’ve charted them all. But this—this is different.”\n\n" +
                    "“The Mountain Stronghold’s paths are old, but something new prowls them now. *MountainBrigand Lord*, they call him. Raids my caravans, steals my ink, my vellum—my people.”\n\n" +
                    "“Without my maps, ships drift blind, and trade falters. *Without justice*, I falter.”\n\n" +
                    "**Slay the MountainBrigand Lord**. Let my quill guide Renika once more, and I will see to it you wield a blade worthy of legends.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the mountains claim another soul. But mark me—when ships founder and charts fade, remember what you turned away.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "He still roams free? My caravans won’t last another week. The maps—*our future*—bleed away with each raid.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone, is he? The mountains echo with silence, and I can breathe again.\n\n" +
                       "You’ve not only avenged my losses—you’ve safeguarded every soul who sails by my charts.\n\n" +
                       "Take this: *Blackrazor*. May it slice through shadows as surely as you’ve carved away this blight.";
            }
        }

        public BrigandsBaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainBrigand), "Mountain Brigand", 1));
            AddReward(new BaseReward(typeof(Blackrazor), 1, "Blackrazor"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Brigand’s Bane'!");
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

    public class NishaMoonson : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BrigandsBaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMapmaker());
        }

        [Constructable]
        public NishaMoonson()
            : base("the Cartographer", "Nisha Moonson")
        {
        }

        public NishaMoonson(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1052; // Slightly sun-kissed
            HairItemID = 0x2048; // Long hair
            HairHue = 1154; // Deep sea blue
        }

        public override void InitOutfit()
        {
            AddItem(new ElvenShirt() { Hue = 1372, Name = "Mariner's Silk Shirt" }); // Ocean-teal
            AddItem(new LeatherLegs() { Hue = 2101, Name = "Navigator's Breeches" }); // Weathered brown
            AddItem(new HalfApron() { Hue = 1342, Name = "Mapmaker's Apron" }); // Ink-stained grey
            AddItem(new Cloak() { Hue = 1175, Name = "Cape of the Northern Winds" }); // Deep sea green
            AddItem(new ThighBoots() { Hue = 1171, Name = "Tidewalkers" }); // Salt-washed leather
            AddItem(new FeatheredHat() { Hue = 2106, Name = "Cartographer’s Crest" }); // Feathered navy hat

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Map Satchel";
            AddItem(backpack);

            AddItem(new ScribeSword() { Hue = 2418, Name = "Inkblade" }); // Quill-tipped sword
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
