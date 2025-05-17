using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScreechOfDespairQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Screech of Despair"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Lute Songfell*, Dawn’s renowned musician, sitting in his workshop surrounded by shattered lute strings and torn sheet music.\n\n" +
                    "His fingers twitch as if strumming invisible chords, his eyes haunted but defiant.\n\n" +
                    "“I’ve played through storms and sorrow, through laughter and harvest moon—but this? This is *cacophony incarnate.*”\n\n" +
                    "“A creature from the Doom dungeon... a **Doomed Skree**, they call it. Its screech splits the air, and worse—it *tunes* itself to the sounds of joy.”\n\n" +
                    "“Whenever I play, my lute *trembles*—not from beauty, but terror. If it gets closer, no song in Dawn will ever be heard again.”\n\n" +
                    "**Slay the Doomed Skree**, and restore silence to the shadows... so I may bring music back to the light.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may silence reign. But know this—the longer it lives, the fewer will remember the sound of hope.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the Skree wails? Even in sleep, I hear its cry. The strings hum. My hands ache to play—but the music turns to ash.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You silenced it? Truly?\n\n" +
                       "Listen... nothing. *Blessed nothing.*\n\n" +
                       "Now, the song can rise again—and for you, a gift: the *RingsOfTheLostTide*. May they carry you on winds no screech shall taint.";
            }
        }

        public ScreechOfDespairQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomedSkree), "Doomed Skree", 1));
            AddReward(new BaseReward(typeof(RingsOfTheLostTide), 1, "RingsOfTheLostTide"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Screech of Despair'!");
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

    public class LuteSongfell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScreechOfDespairQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public LuteSongfell()
            : base("the Troubled Musician", "Lute Songfell")
        {
        }

        public LuteSongfell(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 60, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Light rose skin
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Soft silver
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1157, Name = "Chorusweave Tunic" }); // Midnight blue
            AddItem(new LongPants() { Hue = 2412, Name = "Ballad-Stitched Trousers" }); // Dusky lavender
            AddItem(new Boots() { Hue = 2101, Name = "Echohide Boots" }); // Deep onyx
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Hat of Quieted Chords" }); // Faded teal
            AddItem(new Cloak() { Hue = 1170, Name = "Mantle of Harmonic Silence" }); // Pale grey

            AddItem(new ResonantHarp() { Hue = 2500, Name = "Harp of the Fractured Note" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Songfell’s Satchel";
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
