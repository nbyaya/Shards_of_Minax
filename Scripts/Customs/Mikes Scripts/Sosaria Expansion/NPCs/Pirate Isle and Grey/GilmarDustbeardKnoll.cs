using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ArcanistsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Arcanist's End"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Gilmar 'Dustbeard' Knoll*, Cartographer of Pirate Isle, hunched over a table of burned parchment scraps.\n\n" +
                    "His beard, flecked with ink and ash, twitches as he notices your gaze.\n\n" +
                    "\"You ever seen a map *erase itself*? No? Then you’ve never charted near Exodus Castle. I did. Once. Marked every inch, 'til some *Dustbound Arcanist* set it alight with naught but a glare. My life's work—gone in smoke and cinders.\"\n\n" +
                    "\"But I know where it lairs now. And I know what it holds—a grimoire. My grimoire. My charts, wrapped in spell and fury. Bring it back. Burn that wretch to dust. And I’ll reward you well... with a stand to rest your weary head or wig, whichever you prefer.\"\n\n" +
                    "**Slay the Dustbound Arcanist** and reclaim the lost arcane grimoire.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Then leave me to my ashes and regrets. The winds won’t blow kindly for cowards.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"The Arcanist still breathes? Then every map I draw shall burn again. End this. For all our sakes.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"You’ve done it. By the salt and stars, you’ve done it. My charts... my mind’s map... they’ll live again. Here, take this *WigStand*. Let it hold your helm high. You've earned that much, and more.\"";
            }
        }

        public ArcanistsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DustboundArcanist), "Dustbound Arcanist", 1));
            AddReward(new BaseReward(typeof(WigStand), 1, "WigStand"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Arcanist's End'!");
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

    public class GilmarDustbeardKnoll : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ArcanistsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHairStylist()); // Cartographer profession
        }

        [Constructable]
        public GilmarDustbeardKnoll()
            : base("the Ashen Cartographer", "Gilmar 'Dustbeard' Knoll")
        {
        }

        public GilmarDustbeardKnoll(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2412; // Weathered tan skin
            HairItemID = 0x2040; // Long hair
            HairHue = 1150; // Ash-grey
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1824, Name = "Ash-Woven Shirt" }); // Faded grey-blue
            AddItem(new LongPants() { Hue = 1109, Name = "Char-Edged Trousers" });
            AddItem(new HalfApron() { Hue = 1908, Name = "Mapmaker's Apron" }); // Ink-stained black
            AddItem(new TricorneHat() { Hue = 2413, Name = "Wind-Worn Tricorne" }); // Weathered grey
            AddItem(new Boots() { Hue = 1812, Name = "Saltcrust Boots" });

            AddItem(new ScribeSword() { Hue = 2500, Name = "Charred Stylus" }); // Blackened with magical fire

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Scrollsack";
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
