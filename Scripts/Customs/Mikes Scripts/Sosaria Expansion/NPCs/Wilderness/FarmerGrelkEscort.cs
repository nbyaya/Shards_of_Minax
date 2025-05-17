using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FarmerGrelkQuest : BaseQuest
    {
        public override object Title { get { return "The Seed of Seasons"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Grelk fidgets with a small, glowing seed in his palm, his weathered face a mix of hope and panic.*\n\n" +
                    "\"Name’s Grelk, friend. I got me hands on somethin’ special—a seed, rare as a unicorn’s tooth! It’s gotta be planted in Green Acres afore the moon turns, or it withers, and I reckon we’ll both be cursed with bad crops forever! Trouble is... I can’t remember the way back. Help an old farmer, will ya?\"";
            }
        }

        public override object Refuse { get { return "*Grelk’s face falls, clutching the seed tighter.* \"Ah... guess it’ll just die with me, then.\""; } }
        public override object Uncomplete { get { return "*Grelk wipes sweat from his brow.* \"C’mon now, the seed’s startin’ to hum funny. We gotta get movin’!\""; } }

        public FarmerGrelkQuest() : base()
        {
            AddObjective(new EscortObjective("Green Acres"));
            AddReward(new BaseReward(typeof(HarvestersHelm), "HarvestersHelm – Increases harvesting efficiency and provides minor nature resistance."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Grelk laughs heartily as he plants the seed, a gentle light spilling over the soil.* \"By the earth, you done good! Here, take this old helm. May it bless yer hands like it did mine. Now... where did I put them carrots?\"", null, 0x59B);
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

    public class FarmerGrelkEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(FarmerGrelkQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public FarmerGrelkEscort() : base()
        {
            Name = "Farmer Grelk";
            Title = "the Seedkeeper";
            NameHue = 0x59B;
        }

		public FarmerGrelkEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 40, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Sun-tanned skin tone
            HairItemID = 0x2049; // Messy hair
            HairHue = 1150; // Straw-blonde
            FacialHairItemID = 0x203F; // Long beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StrawHat() { Hue = 1367, Name = "Green Acres Straw Hat" }); // Earthy green
            AddItem(new FancyShirt() { Hue = 2212, Name = "Fieldhand’s Blouse" }); // Warm tan
            AddItem(new ShortPants() { Hue = 2101, Name = "Soil-Stained Breeches" }); // Dark brown
            AddItem(new HalfApron() { Hue = 1191, Name = "Apron of Seasons" }); // Faded orange
            AddItem(new Boots() { Hue = 1109, Name = "Muck-Treader Boots" }); // Dusty black
            AddItem(new BodySash() { Hue = 1260, Name = "Sash of Verdant Luck" }); // Bright green

            Backpack backpack = new Backpack();
            backpack.Hue = 1190; // Burlap tone
            backpack.Name = "Seedkeeper's Pouch";
            AddItem(backpack);

            // Holds the mythical seed
            Item seed = new SeedOfSeasons();
            seed.Movable = false; // Cannot be taken from his inventory
            AddItem(seed);
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
                        "*Grelk scratches his head.* Now where’d I put that hoe? No matter, let’s keep goin’.",
                        "*He pats his pouch.* This seed’s buzzin’ like a swarm of bees… it ain’t happy waitin’.",
                        "*Grelk chuckles.* Ever see a turnip grow as big as yer head? I aim to beat that record!",
                        "*He looks around nervously.* Ain’t no place like Green Acres... but cursed if I know the way.",
                        "*Grelk mutters.* If this seed don’t make it, the crows’ll laugh me outta the fields!",
                        "*He beams.* With you helpin’, we’ll make it back in time. I just know it!",
                        "*Grelk hums a rustic tune.* Green Acres is the place for me... plantin’ seeds and drinkin’ tea..."
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

    public class SeedOfSeasons : Item
    {
        [Constructable]
        public SeedOfSeasons() : base(0x0C8E) // Unique seed graphic
        {
            Name = "Mythical Seed of Seasons";
            Hue = 1153; // Soft glowing blue
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public SeedOfSeasons(Serial serial) : base(serial) { }

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
