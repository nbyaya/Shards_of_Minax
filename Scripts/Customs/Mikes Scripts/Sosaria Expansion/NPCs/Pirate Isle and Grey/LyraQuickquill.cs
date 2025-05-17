using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FlickersEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Flicker's End"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Lyra Quickquill*, a meticulous scribe of Pirate Isle. She clutches a set of flickering scrolls, eyes wide with a desperate gleam.\n\n" +
                    "“These words… they fade, even as I write. Something in **Exodus Dungeon** devours them—the *FlickeringRemnant*.”\n\n" +
                    "“It's not just parchment. It’s memory, history, thought. Every time it nears, my work crumbles into dust.”\n\n" +
                    "**“Slay the FlickeringRemnant**, and let my words live beyond the shadows.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may my ink hold, though I fear it won’t last long without your help.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it flickers? My scrolls dim with each passing hour… Please, do not tarry.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done? The flicker has stilled?\n\n" +
                       "Bless you, wanderer. My words are safe again, for now.\n\n" +
                       "Take this—*the SwordOfGideon*. It was entrusted to me for safeguarding knowledge, but now I entrust it to you. May it protect you as you protected my legacy.";
            }
        }

        public FlickersEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FlickeringRemnant), "FlickeringRemnant", 1));
            AddReward(new BaseReward(typeof(SwordOfGideon), 1, "SwordOfGideon"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Flicker's End'!");
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

    public class LyraQuickquill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FlickersEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTanner()); // Fits her profession as a scribe
        }

        [Constructable]
        public LyraQuickquill()
            : base("the Scribe", "Lyra Quickquill")
        {
        }

        public LyraQuickquill(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Deep sapphire
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1161, Name = "Ink-Stained Blouse" }); // Midnight blue
            AddItem(new Skirt() { Hue = 2101, Name = "Wordweaver's Skirt" }); // Soft parchment hue
            AddItem(new Shoes() { Hue = 1819, Name = "Scrivener's Slippers" }); // Brown leather
            AddItem(new Cloak() { Hue = 1150, Name = "Flicker's Veil" }); // Ghostly grey
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Quickquill Cap" }); // Dark indigo

            AddItem(new ArtificerWand() { Hue = 1175, Name = "Quill of Binding" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scroll Satchel";
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
