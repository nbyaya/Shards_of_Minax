using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BloodAndIceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Blood and Ice"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Lyra Silverbranch*, a minstrel of Fawn, standing by the frozen fountain in the town square. Frost clings to her flute, and broken instruments hang from her satchel.\n\n" +
                    "“Each note I play... it freezes. Not just the air, but something deeper.”\n\n" +
                    "“The **Sangsleet**, they call it. Born of blood and ice, it wanders the Wilderness, spreading a frost that bleeds. I've seen travelers return pale, trembling, their breath frozen red.”\n\n" +
                    "“I’ve tried to drown it in music, but my songs falter, and my flutes shatter. The Sangsleet must be stopped, or Fawn will suffer a winter we can’t survive.”\n\n" +
                    "**Slay the Sangsleet**, end this hemorrhagic frost, and let music warm the air once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the winds. They carry more than chill—they carry sorrow.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it breathes ice? The frost is creeping further. My songs are brittle, my fingers numb.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The Sangsleet is no more?\n\n" +
                       "The frost fades from my breath... and my music returns. Here, take these: *WhisperersBoots*. Step lightly as frost, and may no song falter for you.";
            }
        }

        public BloodAndIceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Sangsleet), "Sangsleet", 1));
            AddReward(new BaseReward(typeof(WhisperersBoots), 1, "WhisperersBoots"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Blood and Ice'!");
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

    public class LyraSilverbranch : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BloodAndIceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public LyraSilverbranch()
            : base("the Frost-Touched Minstrel", "Lyra Silverbranch")
        {
        }

        public LyraSilverbranch(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 80);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Elf;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1152; // Icy white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Snowfall Blouse" }); // Frost-blue
            AddItem(new Skirt() { Hue = 1150, Name = "Glacier Wrap" }); // Pale icy hue
            AddItem(new Cloak() { Hue = 1109, Name = "Chillwind Cloak" }); // Light silver
            AddItem(new ThighBoots() { Hue = 1151, Name = "Whisper-Leather Boots" });
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Frosted Plume" });

            AddItem(new ResonantHarp() { Hue = 1150, Name = "Echo-Harp of Fawn" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Minstrel’s Satchel";
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
