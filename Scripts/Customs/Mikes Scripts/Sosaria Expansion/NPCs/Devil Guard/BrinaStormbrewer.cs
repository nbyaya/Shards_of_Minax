using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SkyfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Skyfall"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Brina Stormbrewer*, Devil Guard’s steadfast Weather Observer, hunched over an array of storm-measuring instruments.\n\n" +
                    "Her cloak billows, caught by unseen gusts swirling from the mountains.\n\n" +
                    "“The sky weeps iron,” she mutters. “It sings... before it falls.”\n\n" +
                    "“A bird, not natural—**an OreBird**—has nested in the shattered caverns. Its cry brings flurries of metal. My instruments... useless since it came.”\n\n" +
                    "“You’ll find it where the mines quake. Kill it, and I’ll read the skies again. Before the storms turn deadly.”\n\n" +
                    "**Slay the OreBird** in the Mines of Minax and return. I need its feathers to study the storms it brings.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then steel yourself. For the next storm might not come from the sky—it may rise from below.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The OreBird still sings? Each note, each cry—brings the storm closer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The skies fall silent... for now.\n\n" +
                       "With its plumage, I can predict the metal rains. You’ve calmed the mountain’s fury.\n\n" +
                       "Take this: *RainwhisperBrim*. It shields the mind from the storm, and perhaps, reminds the heavens we’re not so easily broken.";
            }
        }

        public SkyfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(OreBird), "OreBird", 1));
            AddReward(new BaseReward(typeof(RainwhisperBrim), 1, "RainwhisperBrim"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Skyfall'!");
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

    public class BrinaStormbrewer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SkyfallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        [Constructable]
        public BrinaStormbrewer()
            : base("the Weather Observer", "Brina Stormbrewer")
        {
        }

        public BrinaStormbrewer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Storm-grey
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Stormwoven Shirt" });
            AddItem(new Skirt() { Hue = 2403, Name = "Skywatcher's Skirt" });
            AddItem(new Cloak() { Hue = 1154, Name = "Galecloak" });
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Rainwhisper Brim" });
            AddItem(new Boots() { Hue = 2101, Name = "Thunderstep Boots" });
            AddItem(new GnarledStaff() { Hue = 2406, Name = "Windreader's Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2106;
            backpack.Name = "Storm Satchel";
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
