using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FrostsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Frost’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Elara Swiftquill*, a meticulous scribe surrounded by scrolls bound in seals of warmth.\n\n" +
                    "She glances up, her quill pausing mid-sentence, eyes sharp beneath her feathered hat.\n\n" +
                    "“The Aurefrost—it breathes ice into the heart of the forest. Its shards once nearly claimed my apprentice… and now they threaten my work.”\n\n" +
                    "“Legends call it a remnant of a forgotten winter god, cursed to wander the woods, freezing all it touches. If it isn't stopped, my scrolls—our history—will be encased in frost, lost forever.”\n\n" +
                    "**Slay the Aurefrost** and reclaim the forest’s warmth. Return to me with its frozen heart, that I may seal away its tale—and its curse—for good.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand. But remember, history fades under ice. And some tales, if left untold, become traps for the unwary.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The cold lingers... Have you not yet faced the Aurefrost? Each hour lost strengthens its grasp.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The heart... frozen, yet no longer beating. You’ve ended its reign.\n\n" +
                       "Take this: *BloodwoodAxe*. Crafted from the heart of ancient trees, it will cleave even through frost as deep as time itself.";
            }
        }

        public FrostsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Aurefrost), "Aurefrost", 1));
            AddReward(new BaseReward(typeof(BloodwoodAxe), 1, "BloodwoodAxe"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Frost’s End'!");
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

    public class ElaraSwiftquill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FrostsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public ElaraSwiftquill()
            : base("the Scribe of Frost-Tales", "Elara Swiftquill")
        {
        }

        public ElaraSwiftquill(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Icy-blue tint
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1150, Name = "Frostweave Gown" }); // Cool silver-blue
            AddItem(new Cloak() { Hue = 1154, Name = "Scrollkeeper’s Mantle" }); // Light icy blue
            AddItem(new FeatheredHat() { Hue = 1152, Name = "Winter's Quill Hat" }); // Pale frost hue
            AddItem(new Sandals() { Hue = 1151, Name = "Icebound Sandals" });

            AddItem(new ScribeSword() { Hue = 2053, Name = "Runeblade Quill" }); // Glows faintly with icy runes

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
