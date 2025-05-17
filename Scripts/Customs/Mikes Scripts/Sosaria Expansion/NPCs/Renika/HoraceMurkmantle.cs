using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WebOfWorldsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Web of Worlds"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Horace Murkmantle*, the shadowed figure hunched over ancient tomes within the Renika Archives.\n\n" +
                    "His robes shimmer faintly, as though inked with stars, and his eyes—deep-set beneath a hood—glimmer with suppressed dread.\n\n" +
                    "“A breach... an interstice in the weave of this world. One we did not foresee, yet one I must now implore you to mend.”\n\n" +
                    "“A creature known as the **NexusWeaver** has nested within the Mountain Stronghold. It devours not flesh, but *knowledge*—ensnaring our most precious scrolls in its dimensional web. These scrolls are *foundational* to Renika’s history, and without them, we risk forgetting who we are.”\n\n" +
                    "“I guard these archives, but my oaths do not arm me against such beasts. You, however, may succeed where I cannot.”\n\n" +
                    "**Slay the NexusWeaver** and return what was lost, before the strands of our world unravel.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we shall watch helplessly as our past is consumed, and with it, perhaps, our future.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The NexusWeaver still clutches our truths in its silken maw? Time is a thread fraying fast, adventurer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The tomes… they are returned! You’ve severed the strands of that blasphemous loom.\n\n" +
                       "Renika’s memory lives, thanks to you. Take this *MilkingPail*—its worth, perhaps humble, but symbolizing nourishment reclaimed from the void.";
            }
        }

        public WebOfWorldsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(NexusWeaver), "NexusWeaver", 1));
            AddReward(new BaseReward(typeof(MilkingPail), 1, "MilkingPail"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Web of Worlds'!");
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

    public class HoraceMurkmantle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WebOfWorldsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this)); // Librarian-like profession
        }

        [Constructable]
        public HoraceMurkmantle()
            : base("the Keeper of Forbidden Lore", "Horace Murkmantle")
        {
        }

        public HoraceMurkmantle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 70, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Pale, almost spectral
            HairItemID = 0x203C; // Long hair
            HairHue = 1109; // Dust-gray
            FacialHairItemID = 0x2041; // Short beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1153, Name = "Void-Wrapped Shroud" });
            AddItem(new ElvenBoots() { Hue = 1157, Name = "Ink-Stained Boots" });
            AddItem(new BodySash() { Hue = 1150, Name = "Archivist's Binding Sash" });

            AddItem(new ScribeSword() { Hue = 1161, Name = "Scriptcutter" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scrollkeeper's Pack";
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
