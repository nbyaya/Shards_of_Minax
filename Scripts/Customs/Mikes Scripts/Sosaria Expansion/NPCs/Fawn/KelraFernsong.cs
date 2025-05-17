using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentEchoesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Echoes"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Kelra Fernsong*, bard of Fawn, clutching a dulcimer with trembling hands.\n\n" +
                    "Her eyes are shadowed, her voice little more than a whisper as she says:\n\n" +
                    "“There’s a sound in the Wilderness—soft, mournful, and wrong. I call it the Cryvyn. I met it once, you know.”\n\n" +
                    "“It came from the mists, wailing. And my song—it *broke*. The music turned on me, and the wildlife... twisted. For weeks, I could not sing, not even hum.”\n\n" +
                    "**“Hunt the Cryvyn. Silence its echoes before more of Fawn’s beauty is lost to its sorrow.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the mists. If you hear its song, cover your ears... and pray it does not notice you.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it cries? Then my dreams will remain broken, and the woods will twist further.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is gone? Truly? I feel the notes returning to me, like birds after a long storm.\n\n" +
                       "You’ve done more than slay a beast—you’ve given me my song back. And with it, hope.\n\n" +
                       "**Take this: SwingTimeChest. May rhythm and grace guide your path as you have restored mine.**";
            }
        }

        public SilentEchoesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Cryvyn), "Cryvyn", 1));
            AddReward(new BaseReward(typeof(SwingTimeChest), 1, "SwingTimeChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Echoes'!");
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

    public class KelraFernsong : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentEchoesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public KelraFernsong()
            : base("the Melancholic Bard", "Kelra Fernsong")
        {
        }

        public KelraFernsong(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(65, 75, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long Hair
            HairHue = 1153; // Deep Indigo
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1372, Name = "Twilight Serenade Gown" }); // Dusky lavender
            AddItem(new FeatheredHat() { Hue = 1157, Name = "Moonlit Minstrel's Cap" }); // Midnight blue
            AddItem(new Cloak() { Hue = 1175, Name = "Whispercloak" }); // Faint silver
            AddItem(new ThighBoots() { Hue = 1109, Name = "Soft-Stepped Boots" }); // Grey
            AddItem(new BodySash() { Hue = 1171, Name = "Songweaver's Sash" }); // Pale blue

            AddItem(new ResonantHarp() { Hue = 2101, Name = "Dulcimer of Echoes" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bard's Satchel";
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
