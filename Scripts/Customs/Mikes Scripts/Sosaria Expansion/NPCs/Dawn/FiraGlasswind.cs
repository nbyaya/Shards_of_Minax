using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShatterTheShaperQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shatter the Shaper"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Fira Glasswind*, master glassblower of Dawn, her eyes reflecting fractured light, hands trembling slightly as she polishes a warped vial.\n\n" +
                    "“I’ve shaped flame and sand all my life,” she says, voice like the clinking of delicate crystal. “But now, my art betrays me.”\n\n" +
                    "“Something—**an Entropy Elemental**—lurks near Doom, corrupting my creations. My goblets melt in hand, my vials burst without cause. I’ve seen tools twist, surfaces ripple like water.”\n\n" +
                    "“I thought it madness at first, but I *felt* it—this corrosive presence, warping more than glass. It’s in the air, in the earth. It wants to unmake.”\n\n" +
                    "**Slay the Entropy Elemental** that haunts Doom, or soon my craft—and perhaps Dawn itself—will be shattered beyond repair.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to my splinters. But beware—what breaks my glass may soon fracture the world you walk.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it shreds the air? I cannot work. The glass sings of its presence—it won’t let me shape or breathe.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? I can *feel* it—like a breath held too long, now finally released.\n\n" +
                       "The furnace calms. My glass holds true again. You’ve not only slain a beast—you’ve preserved a craft born of fire and light.\n\n" +
                       "Take this: *Isobel’s Whisper*. Crafted in thanks, blessed in flame, strong against what seeks to warp and consume.";
            }
        }

        public ShatterTheShaperQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EntropyElemental), "Entropy Elemental", 1));
            AddReward(new BaseReward(typeof(IsobelsWhisper), 1, "Isobel’s Whisper"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shatter the Shaper'!");
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

    public class FiraGlasswind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShatterTheShaperQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGlassblower());
        }

        [Constructable]
        public FiraGlasswind()
            : base("the Glassblower", "Fira Glasswind")
        {
        }

        public FiraGlasswind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 30);

            Female = true;
            Body = 0x191; // Female Human
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Pale icy blue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 1150, Name = "Glassweaver’s Robe" }); // Iridescent white-blue
            AddItem(new HalfApron() { Hue = 1341, Name = "Cinderspun Apron" }); // Soft amber hue
            AddItem(new Sandals() { Hue = 1109, Name = "Ashen Sandals" }); // Light grey
            AddItem(new Cloak() { Hue = 1153, Name = "Whispercloak of Dawn" }); // Pale icy blue matching hair

            AddItem(new Mace() { Hue = 1348, Name = "Molten Threader" }); // Tool, glowing softly with hues of orange-pink glass
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
