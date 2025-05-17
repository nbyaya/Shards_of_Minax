using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilenceInfernalBansheeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silence the InfernalBanshee"; } }

        public override object Description
        {
            get
            {
                return
                    "You find **Nessa Gale**, the bard of West Montor, strumming her lute in frustration, her notes faltering under the weight of a sorrowful presence.\n\n" +
                    "\"I cannot sing... not while **her** lament echoes from the Gate of Hell. The *InfernalBanshee*, they call her. But she was once like me—a voice of joy turned to woe. Now her wails twist every melody, unmake every song I know.\"\n\n" +
                    "\"I sought Mother Edda's wisdom once, traded her a piece of my soul to keep my lute safe from curses. But now, no charm can drown out that scream.\"\n\n" +
                    "**Slay the InfernalBanshee** and bring silence back to the wind, so that music may rise again.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Then may silence reign, and the InfernalBanshee have her song alone. I will bury my voice in the earth if I must.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still she sings? I feel her in my throat now, in my dreams. She steals more than song—she steals breath.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"The wails have ceased? I feel... light again. Like my voice is my own.\"\n\n" +
                       "\"Take this—**Grimmblade**. A song in steel, forged by sorrow but wielded with hope. May your strikes be true, and your spirit unbroken.\"";
            }
        }

        public SilenceInfernalBansheeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernalBanshee), "InfernalBanshee", 1));
            AddReward(new BaseReward(typeof(Grimmblade), 1, "Grimmblade"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silence the InfernalBanshee'!");
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

    public class NessaGale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilenceInfernalBansheeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public NessaGale()
            : base("the Town Bard", "Nessa Gale")
        {
        }

        public NessaGale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2048; // Long hair
            HairHue = 1153; // Soft lavender
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 137, Name = "Lament-Weaver’s Gown" }); // Deep violet
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Twilight Minstrel’s Cap" }); // Night-blue
            AddItem(new Shoes() { Hue = 2401, Name = "Duskwalkers" }); // Soft grey
            AddItem(new Cloak() { Hue = 1260, Name = "Songweaver’s Shroud" }); // Pale silver
            AddItem(new BodySash() { Hue = 1157, Name = "Sash of Silent Notes" }); // Faded blue
            AddItem(new ResonantHarp() { Hue = 1150, Name = "Wailbreaker" }); // Ghostly white harp

            Backpack backpack = new Backpack();
            backpack.Hue = 1161;
            backpack.Name = "Melody Satchel";
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
