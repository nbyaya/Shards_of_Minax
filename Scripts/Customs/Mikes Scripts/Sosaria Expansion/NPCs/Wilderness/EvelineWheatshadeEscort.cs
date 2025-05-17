using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EvelineWheatshadeQuest : BaseQuest
    {
        public override object Title { get { return "A Taste of Home"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Eveline’s warm smile falters, her hands clutching a woven sack of exotic grains.*\n\n" +
                    "\"Greetings, traveler. I am Eveline Wheatshade, baker of Dawn. I've journeyed far to gather these rare grains for my next creation... but some would see my recipes lost or stolen. Would you walk with me, back to Dawn, where hearth and home await?\"";
            }
        }

        public override object Refuse { get { return "*Eveline nods, though her eyes betray worry.* \"Very well. May the winds guide me safely, alone.\""; } }
        public override object Uncomplete { get { return "*She adjusts the sack on her shoulder.* \"We mustn't tarry; the warmth of my oven calls.\""; } }

        public EvelineWheatshadeQuest() : base()
        {
            AddObjective(new EscortObjective("Dawn"));
            AddReward(new BaseReward(typeof(FishBasket), "FishBasket – Produces small bonuses to food-related skills and luck when fishing."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Eveline beams as she steps into Dawn.* \"Thank you, friend. My hearth is safe, and with it, the taste of home. Please, take this basket—it brings fortune to those with a love for the sea and the table.\"", null, 0x59B);
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

    public class EvelineWheatshadeEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(EvelineWheatshadeQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public EvelineWheatshadeEscort() : base()
        {
            Name = "Eveline Wheatshade";
            Title = "the Master Baker";
            NameHue = 0x58B; // Warm golden
        }

		public EvelineWheatshadeEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 50, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1020; // Fair complexion
            HairItemID = 0x203B; // Braided hair
            HairHue = 2101; // Wheat-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Sunflour Blouse" }); // Soft yellow
            AddItem(new HalfApron() { Hue = 2125, Name = "Wheatshade Apron" }); // Rich golden-brown
            AddItem(new LongPants() { Hue = 1109, Name = "Kneaded Breeches" }); // Flour-dusted grey
            AddItem(new Shoes() { Hue = 1171, Name = "Hearthwalkers" }); // Warm brown
            AddItem(new BodySash() { Hue = 1141, Name = "Grainbinder's Sash" }); // Deep orange
            AddItem(new Bonnet() { Hue = 2124, Name = "Baker’s Bonnet" }); // Earthy tan

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Grain-Sack";
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
                        "*Eveline hums a warm tune about hearth and home.*",
                        "*She checks her grain sack.* These grains will make bread like no other... if they stay safe.",
                        "*Eveline looks around nervously.* Some folk want more than a loaf—they want my secrets.",
                        "*She smiles softly.* Dawn’s ovens wait, and the festival must go on.",
                        "*The smell of flour seems to follow her.* A hint of vanilla... or is it lavender?",
                        "*She chuckles.* You’d be surprised how many adventures begin over a slice of bread.",
                        "*Eveline adjusts her bonnet.* I once baked for kings, but the simple folk of Dawn are my true kin."
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
