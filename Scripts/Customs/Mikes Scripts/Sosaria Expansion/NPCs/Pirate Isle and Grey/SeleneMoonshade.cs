using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RestlessDeadQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Restless Dead"; } }

        public override object Description
        {
            get
            {
                return
                    "*Selene Moonshade*, the Night Watcher of Grey, greets you under the dim light of a flickering lantern.\n\n" +
                    "Her eyes never leave the horizon as she speaks, \"She paces still, you know... the lady of the tower. The sailors see her, the soldiers hear her. **The RestlessSpirit.**\"\n\n" +
                    "\"Each night her shadow crosses the lookout. Each night, someone new is taken by dreams they do not wake from.\"\n\n" +
                    "\"I've rung this silver bell more times than I care to count, but no aid has come. **Will you silence her?** The dead must rest, or none of us shall.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then she will walk the ramparts still, and the sea will carry more souls to her call.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The tower still weeps with her sorrow. Have you not faced her?";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"The bell no longer rings at night... You’ve given us peace, if only for a time.\"\n\n" +
                       "Selene hands you a gift, her voice soft, \"Take this, and remember that some spirits sleep only when watched by kind eyes.\"";
            }
        }

        public RestlessDeadQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RestlessSpirit), "RestlessSpirit", 1));
            AddReward(new BaseReward(typeof(ChefsHatOfFocus), 1, "Chef's Hat of Focus"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Restless Dead'!");
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

    public class SeleneMoonshade : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RestlessDeadQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller());
        }

        [Constructable]
        public SeleneMoonshade()
            : base("the Night Watcher", "Selene Moonshade")
        {
        }

        public SeleneMoonshade(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, moonlit skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1109, Name = "Moonshade Cloak" });
            AddItem(new FancyDress() { Hue = 1157, Name = "Stargazer’s Gown" });
            AddItem(new Sandals() { Hue = 1150, Name = "Silent Steps" });
            AddItem(new BodySash() { Hue = 1153, Name = "Watcher’s Sash" });

            AddItem(new Lantern() { Hue = 2101, Name = "Warding Light" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Watcher’s Pack";
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
