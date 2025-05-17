using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EchoedClamorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Echoed Clamor"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Yara Silverquill*, Poet of Moon, who dwells in the shadow of the ancient Pyramid.\n\n" +
                    "She stands draped in shimmering silks, her eyes distant, haunted by sounds that cannot be silenced.\n\n" +
                    "“The words won’t come anymore… not with that *Rouser* blaring its vile call across the sands.”\n\n" +
                    "“Each note it sounds from the Pyramid’s depths—each *blast*—turns dreams into riots, peace into paranoia. I see it in the faces of Moon’s people. Their sleep shattered, their minds unmoored.”\n\n" +
                    "“I pen verses to soothe. I write so we may remember beauty… but how can I create with that trumpet drowning every thought?”\n\n" +
                    "**Find the Pyramid Rouser**, silence its horn, and give Moon back its dreams. Let my voice return to the winds.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then we shall keep listening to the blare, and I… I shall try to write in the silence between.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“The trumpet still sounds? No quill can write against that din. Moon will not endure much longer if the Rouser sings unchecked.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“It’s gone? Truly? The Pyramid stills at last?”\n\n" +
                       "“You’ve returned not just with victory, but with silence… blessed silence. Listen—can you hear it? The sands are still, and the stars whisper again.”\n\n" +
                       "“Take these—*SageWrappedLimbs*. They were meant for a poet, but I think your deeds are the stuff of legend. Let them guide your hands as they do my words.”";
            }
        }

        public EchoedClamorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PyramidRouser), "Pyramid Rouser", 1));
            AddReward(new BaseReward(typeof(SageWrappedLimbs), 1, "SageWrappedLimbs"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Echoed Clamor'!");
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

    public class YaraSilverquill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EchoedClamorQuest) }; } }
        
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;
		

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer()); 
        }

        [Constructable]
        public YaraSilverquill()
            : base("the Poet of Moon", "Yara Silverquill")
        {
        }

        public YaraSilverquill(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 80);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale, moonlit skin
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Silvery white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1153, Name = "Moonlit Robe" }); // Silvery-blue shimmer
            AddItem(new Cloak() { Hue = 1150, Name = "Starlight Cloak" }); // Deep midnight blue
            AddItem(new Sandals() { Hue = 1170, Name = "Silent Steps" }); // Ethereal gray
            AddItem(new WizardsHat() { Hue = 1151, Name = "Quill-Crested Hat" }); // Pale blue

            AddItem(new ScribeSword() { Hue = 1154, Name = "Silverquill Blade" }); // A ceremonial, stylized weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Poet's Satchel";
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
