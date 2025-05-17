using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DorrikStonebellyQuest : BaseQuest
    {
        public override object Title { get { return "Return to the Depths"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Dorrik brushes dust from his scorched beard, eyes burning with stubborn resolve.*\n\n" +
                    "\"Name's Dorrik Stonebelly. Was diggin’ deep when the ground betrayed me—sealed me in with a shard o' livin' ore, pulsin' like a heartbeat. Need t’ get back t' the cursed mine, lad, finish what I started... else this shard’ll eat me from within. Ye’ll guide me, aye? Through fire, stone, and shadow.\"";
            }
        }

        public override object Refuse { get { return "*Dorrik grunts, clutching the shard tightly.* \"Then may the stone take me whole.\""; } }
        public override object Uncomplete { get { return "*Dorrik winces, the shard glowing faintly.* \"This ore... it stirs. We’ve got t’ keep movin’.\""; } }

        public DorrikStonebellyQuest() : base()
        {
            AddObjective(new EscortObjective("the Mines Of Morinia Dungeon"));
            AddReward(new BaseReward(typeof(PlasmaInfusedWarHammer), "PlasmaInfusedWarHammer – A brutal weapon pulsating with volatile energy."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Dorrik slams the shard into the earth, its glow fading.* \"Bless ye, friend. The mine’s curse’ll not take me this day. Take this—may it strike true in dark places.\"", null, 0x59B);
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

    public class DorrikStonebellyEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(DorrikStonebellyQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public DorrikStonebellyEscort() : base()
        {
            Name = "Dorrik Stonebelly";
            Title = "the Cursed Miner";
            NameHue = 0x480;
        }

		public DorrikStonebellyEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 50, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2412; // Rugged, tanned
            HairItemID = 0x203C; // Long hair
            HairHue = 1109; // Dark, smoky grey
            FacialHairItemID = 0x204D; // Full beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2101, Name = "Stonebelly's Reinforced Vest" }); // Charred bronze
            AddItem(new LeatherLegs() { Hue = 1824, Name = "Coal-Hardened Greaves" }); // Soot-black
            AddItem(new LeatherGloves() { Hue = 1107, Name = "Miner’s Grip" }); // Dusty brown
            AddItem(new StuddedGorget() { Hue = 1840, Name = "Neckguard of Echoes" }); // Dull steel-blue
            AddItem(new LeatherCap() { Hue = 2105, Name = "Dorrik's Craghelm" }); // Rusted red
            AddItem(new Boots() { Hue = 1816, Name = "Deep-Treader Boots" }); // Earthy grey-brown
            AddItem(new BodySash() { Hue = 2115, Name = "Shard-Bound Sash" }); // Faintly glowing green

            AddItem(new WarHammer() { Hue = 2100, Name = "Ore-Splitter" }); // Heavy iron grey

            Backpack backpack = new Backpack();
            backpack.Hue = 1820;
            backpack.Name = "Miner’s Pack";
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
                        "*Dorrik clutches his side.* This shard... it's alive, I tell ye.",
                        "*He scans the shadows nervously.* Somethin' down there still watches... waitin'.",
                        "*He pounds his hammer into his palm.* We dwarves don’t run. We finish what we start.",
                        "*You hear the shard pulse softly.* We best hurry—before it wakes the mine’s heart.",
                        "*Dorrik mutters.* If I fall, bury me deep in the stone, not in cursed light.",
                        "*The ground trembles faintly.* Did ye feel that? The mine never rests."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
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
