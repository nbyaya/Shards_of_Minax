using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StripesSilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Stripe's Silence"; } }

        public override object Description
        {
            get
            {
                return
                    "*Lena Ghostgaze* stands by the edge of Grey’s bustling port, her eyes like storm-lit glass, distant but sharp.\n\n" +
                    "Her cloak ripples with sea winds, but her voice is steady.\n\n" +
                    "“You heard about the caravans? Thought it was bandits too, at first. But my hounds... they won’t go near the trail. They ran when the moon hit that thing’s pelt—turned silver, ghostlike. It leaves no tracks, just whispers that drive animals mad.”\n\n" +
                    "“The *GhoststripeLeopard*. It hunts near Exodus Dungeon now, bold as ever. Last night, I followed it. Watched it slip into shadow. One moment it’s there, next, it’s like smoke. But I swear, I heard it breathe.”\n\n" +
                    "“Bring it down, and I’ll know peace. My dogs too.”\n\n" +
                    "**Slay the GhoststripeLeopard** before more lives vanish into its silent hunt.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Some prey doesn’t care if we turn away. It’ll hunt all the same.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven’t found it? It’s out there still... I can feel the wind shift when it stirs.";
            }
        }

        public override object Complete
        {
            get
            {
                return "*Lena nods, relief flickering across her face like a dying lantern.*\n\n" +
                       "“You did it, didn’t you? The night’s quieter. My hounds slept sound. And me... I owe you.”\n\n" +
                       "“Take this: the *ApronOfFlames*. It’s old, but true. May it keep you warm, even in the darkest trails.”";
            }
        }

        public StripesSilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GhoststripeLeopard), "GhoststripeLeopard", 1));
            AddReward(new BaseReward(typeof(ApronOfFlames), 1, "ApronOfFlames"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Stripe's Silence'!");
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

    public class LenaGhostgaze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StripesSilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public LenaGhostgaze()
            : base("the Tracker", "Lena Ghostgaze")
        {
        }

        public LenaGhostgaze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Storm Grey
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1825, Name = "Ghost-Hunter's Tunic" }); // Shadow Grey
            AddItem(new LeatherLegs() { Hue = 1819, Name = "Moonlight Trousers" });
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Tracker's Grips" });
            AddItem(new Cloak() { Hue = 1153, Name = "Mistcloak" }); // Pale Blue
            AddItem(new Bandana() { Hue = 1157, Name = "Whisperwrap" });
            AddItem(new Boots() { Hue = 1109, Name = "Trailwalkers" });
            AddItem(new CompositeBow() { Hue = 2000, Name = "Silencepiercer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1108;
            backpack.Name = "Tracker's Pouch";
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
