using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SonglessGaleQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Songless Gale"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Lyra Shellsong*, the Siren Archivist of Moon.\n\n" +
                    "Her voice trembles, barely a whisper over the desert winds.\n\n" +
                    "“The sands... they no longer sing. A dryad of the dunes has twisted the sacred winds that once carried our chants.”\n\n" +
                    "Her gaze turns sharp, resolute.\n\n" +
                    "**Slay the Dryad of the Dunes**. Restore the melody of the Moon.”";
            }
        }

        public override object Refuse { get { return "The winds will mourn in silence until a voice rises to challenge her corruption."; } }

        public override object Uncomplete { get { return "The dryad still howls across the dunes? I cannot bear this silence much longer."; } }

        public override object Complete { get { return "The winds stir once more... their song returns. Take this—wrought from golden echoes of old."; } }

        public SonglessGaleQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DryadOfTheDunes), "Dryad of the Dunes", 1));
            AddReward(new BaseReward(typeof(GoldenwroughtElegy), 1, "GoldenwroughtElegy"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Songless Gale'!");
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

    public class LyraShellsong : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SonglessGaleQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHealer()); 
        }

        [Constructable]
        public LyraShellsong()
            : base("the Siren Archivist", "Lyra Shellsong")
        {
        }

        public LyraShellsong(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 30);

            Female = true;
            Body = 0x191; // Female Body
            Race = Race.Human;

            Hue = 1023; // Light sun-kissed tone
            HairItemID = 0x203B; // Long wavy hair
            HairHue = 1153; // Shimmering light blue
        }

        public override void InitOutfit()
        {
            // Unique Outfit
            AddItem(new FancyDress() { Hue = 1150, Name = "Veil of Whispering Sands" }); // Soft moonlit silver
            AddItem(new Cloak() { Hue = 2413, Name = "Songwind Cloak" }); // Pale gold with a shimmer
            AddItem(new Sandals() { Hue = 2406, Name = "Dune-Kissed Sandals" }); // Warm sandstone hue
            AddItem(new FlowerGarland() { Hue = 1154, Name = "Garland of Silent Stars" }); // Delicate blue flowers
            AddItem(new SpellWeaversWand() { Hue = 2421, Name = "Archivist’s Echo" }); // Ornate wand with musical glyphs

            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Windbound Satchel";
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
