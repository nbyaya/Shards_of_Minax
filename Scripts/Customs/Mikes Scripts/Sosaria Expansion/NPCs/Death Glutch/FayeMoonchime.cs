using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CacophonyOfCursesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cacophony of Curses"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Faye Moonchime*, Bard of Death Glutch, her fingers constantly plucking silent chords from a harp strung with moonlit threads.\n\n" +
                    "She stares into the middle distance, her voice light yet strained.\n\n" +
                    "“He sings, and they follow. My songs fade in his shadow, my crowds lost to madness and mirthless laughter.”\n\n" +
                    "**“The Pixie Songlord.** A cursed minstrel who escaped Malidor’s broken halls. His tunes taint the air, twist minds, leave dreams in dissonance.”\n\n" +
                    "“I’ve tried... oh, how I’ve tried. But no melody of mine can drown him. Unless—”\n\n" +
                    "She hums a haunting phrase, one you’ve heard whispered by the wind at night.\n\n" +
                    "“This tune... it calls him. He knows me now. If you strike when he answers—**you can end this**.”\n\n" +
                    "**Slay the Pixie Songlord** in the Malidor Witches Academy, and let silence fall where madness reigned.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then let the Songlord sing, and I shall hum alone—until silence claims us both.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still he sings? Then still I suffer. The nights are longer now... and the tune, it follows me.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "**“He is silent?”** Faye’s eyes widen, her fingers stilling at last.\n\n" +
                       "“You’ve done it. The air feels lighter, the night—quieter. I can hear my own voice again.”\n\n" +
                       "“Take this: *FountainWall.* May its melody remind you that not all songs are curses.”";
            }
        }

        public CacophonyOfCursesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PixieSonglord), "Pixie Songlord", 1));
            AddReward(new BaseReward(typeof(FountainWall), 1, "FountainWall"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Cacophony of Curses'!");
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

    public class FayeMoonchime : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CacophonyOfCursesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public FayeMoonchime()
            : base("the Bard of Death Glutch", "Faye Moonchime")
        {
        }

        public FayeMoonchime(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Long Hair
            HairHue = 1153; // Soft Lavender
            FacialHairItemID = 0; // None
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1260, Name = "Moonweave Gown" }); // Pale Blue-Grey
            AddItem(new Cloak() { Hue = 1175, Name = "Duskwind Cloak" }); // Midnight Blue
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Chimefeather Cap" }); // Lavender
            AddItem(new Sandals() { Hue = 1109, Name = "Minstrel's Steps" }); // Dust Grey

            AddItem(new ResonantHarp() { Hue = 1170, Name = "Whisperstring Harp" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Songweaver’s Satchel";
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
