using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ThunderfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Thunderfall"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Myra Duskmantle*, the revered weaver of Fawn, her fingers stained with dye and her eyes sharp with worry.\n\n" +
                    "She runs her hand over a warped rug, metal threads embedded in the fibers catching the light.\n\n" +
                    "“The *Shaktarro*... it tramples closer each night. I feel its steps through my loom. The threads twist. The colors fade.”\n\n" +
                    "“I’ve spent years weaving the grandest rugs, the soul of Fawn’s halls—but each roar of that beast unravels them. Soon, the looms themselves will splinter.”\n\n" +
                    "“It’s not just fabric. It’s history. Memory. You must **slay the Shaktarro** before its thunderous approach leaves our craft in ruin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I will weave in silence... until there is nothing left but threads and thunder.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still roams? I hear it now, in the weft and warp... it mocks me through the cloth.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The loom is silent again... and my rugs, they hold steady.\n\n" +
                       "You’ve not just slain a beast—you’ve preserved the heart of our craft.\n\n" +
                       "Take this: *MysticGardenCache*. May it bloom wherever you walk.";
            }
        }

        public ThunderfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Shaktarro), "the Shaktarro", 1));
            AddReward(new BaseReward(typeof(MysticGardenCache), 1, "MysticGardenCache"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Thunderfall'!");
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

    public class MyraDuskmantle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThunderfallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaver());
        }

        [Constructable]
        public MyraDuskmantle()
            : base("the Master Weaver", "Myra Duskmantle")
        {
        }

        public MyraDuskmantle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Subtle twilight hue
            HairItemID = 0x203B; // Long hair
            HairHue = 1157; // Dark midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1164, Name = "Duskwoven Gown" }); // Soft violet shade
            AddItem(new Cloak() { Hue = 1109, Name = "Thread of Storms" }); // Thunder-gray
            AddItem(new Sandals() { Hue = 2418, Name = "Weaver's Soles" }); // Deep indigo
            AddItem(new BodySash() { Hue = 1175, Name = "Loombinder's Sash" }); // Metallic silver
            AddItem(new FeatheredHat() { Hue = 1161, Name = "Nightloom Cap" }); // Plum-purple with dark feathers
            AddItem(new MagicWand() { Hue = 2406, Name = "Threadreader's Rod" }); // Light-infused wand symbolizing weaving magic

            Backpack backpack = new Backpack();
            backpack.Hue = 1171; // Subtle shimmer
            backpack.Name = "Weaver's Toolkit";
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
