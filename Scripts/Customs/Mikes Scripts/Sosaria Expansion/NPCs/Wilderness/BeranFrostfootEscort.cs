using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BeranFrostfootQuest : BaseQuest
    {
        public override object Title { get { return "Frost on the Wind"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Beran grips his frostbitten side, eyes narrowed against the wind.*\n\n" +
                    "They’re coming... from the peaks... ice beasts, like none I’ve ever seen. My wounds slow me, and if I don’t warn the inn, they’ll be caught unawares. Will you guide me down the path to Mountain Crest Inn? I must reach them before the frost takes us all.";
            }
        }

        public override object Refuse { get { return "*Beran’s breath fogs in the cold.* Then may the mountain take me, and the inn fall to silence."; } }
        public override object Uncomplete { get { return "*Beran winces.* We’ve no time to waste... the cold will not wait."; } }

        public BeranFrostfootQuest() : base()
        {
            AddObjective(new EscortObjective("Mountain Crest Inn"));
            AddReward(new BaseReward(typeof(Radical90sRelicsChest), "Radical90sRelicsChest – Quirky, powerful relics from a strange era."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Beran clasps your arm firmly.* You’ve saved more than me, friend. These relics... I found them in the frost, strange but potent. Take them, may they serve you well.", null, 0x59B);
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

    public class BeranFrostfootEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(BeranFrostfootQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public BeranFrostfootEscort() : base()
        {
            Name = "Beran Frostfoot";
            Title = "the Mountain Guide";
            NameHue = 1153; // Icy blue name hue
        }

		public BeranFrostfootEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2502; // Pale mountain-tanned skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1150; // Frosted white
            FacialHairItemID = 0x203E; // Beard
            FacialHairHue = 1150; // Frosted white
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1109, Name = "Frostbitten Wrap" }); // Snow-dusted grey
            AddItem(new Cloak() { Hue = 1154, Name = "Cloak of the Glacial Wind" }); // Pale icy blue
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Chill-Grip Gloves" }); // Cold grey
            AddItem(new FurBoots() { Hue = 1102, Name = "Snowbound Boots" }); // Frosty white
            AddItem(new LeatherCap() { Hue = 1151, Name = "Beran's Hood of the Peaks" }); // Faded azure
            AddItem(new QuarterStaff() { Hue = 1157, Name = "Ice-Touched Walking Staff" }); // Pale crystal blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Pack of Warnings";
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
                        "*Beran stumbles, gripping his side.* We must move... the frost is alive...",
                        "*He glances at the peaks.* They hunt in the storm... follow the wind, they said...",
                        "*His breath comes in shallow gasps.* The inn’s fire... it’s the only hope.",
                        "*Beran peers at the horizon.* Once, I climbed for joy... now I flee for my life.",
                        "*Snowflakes cling to his beard.* The beasts... they weren’t always like this... something woke them.",
                        "*He grips his staff tightly.* This staff... it vibrates when they draw near...",
                        "*Beran winces.* Don’t let me fall here... not before the warning is heard."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
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
