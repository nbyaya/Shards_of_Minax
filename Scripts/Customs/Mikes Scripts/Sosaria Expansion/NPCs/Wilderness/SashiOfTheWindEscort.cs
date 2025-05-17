using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SashiOfTheWindQuest : BaseQuest
    {
        public override object Title { get { return "The Last Oasis"; } }

        public override object Description
        {
            get
            {
                return
                    "*Sashi stands tall, eyes fierce, but the weight of time presses upon her shoulders.*\n\n" +
                    "I am Sashi of the Wind, bearer of the last breath of the Sandsong Oasis. My people thirst, and I must return this sacred water before the dunes reclaim our home. The winds grow wild, and shadows follow. Will you walk with me, across the sands, before all is lost?";
            }
        }

        public override object Refuse { get { return "*Sashi bows slightly, her voice dry as the desert.* Then may the winds show me mercy alone."; } }
        public override object Uncomplete { get { return "*Her pace quickens, urgency in her voice.* The sands are rising... we must go on."; } }

        public SashiOfTheWindQuest() : base()
        {
            AddObjective(new EscortObjective("the Shingorr Village"));
            AddReward(new BaseReward(typeof(CourtesansGracefulKimono), "Courtesan's Graceful Kimono – Elegant attire boosting speed and evasion."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Sashi kneels, pouring a drop of water into the earth.* You have carried the winds with me. Take this gift – it will make you fleet as the desert breeze. May you always find your path, even in shifting sands.", null, 0x59B);
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

    public class SashiOfTheWindEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SashiOfTheWindQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SashiOfTheWindEscort() : base()
        {
            Name = "Sashi of the Wind";
            Title = "Desert Nomad";
            NameHue = 0x47F;
        }

		public SashiOfTheWindEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1039; // Sun-kissed skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Sandy blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleKimono() { Hue = 1354, Name = "Kimono of the Wandering Wind" }); // Pale sand color with blue trim
            AddItem(new Obi() { Hue = 1153, Name = "Skybound Sash" }); // Light blue belt
            AddItem(new Sandals() { Hue = 1171, Name = "Dune Striders" }); // Light tan
            AddItem(new FeatheredHat() { Hue = 1109, Name = "Nomad's Shade" }); // Dusty grey, wide-brimmed
            AddItem(new Cloak() { Hue = 1353, Name = "Mantle of the Oasis" }); // Soft green cloak, representing life in the desert

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Waterskin Satchel";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*Sashi adjusts her waterskin.* The sands shift, as do fates. We must move swiftly.",
                        "*Her eyes scan the horizon.* The wind speaks. It tells of storms ahead.",
                        "*She hums a desert tune.* This song has guided my steps since I was a child.",
                        "*Sashi whispers.* The dunes remember those who wander too long...",
                        "*She holds a small vial aloft.* This is life. This is hope. We cannot fail.",
                        "*Sashi glances at you.* Have you ever lost a home to the desert? Pray you do not."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }
}
