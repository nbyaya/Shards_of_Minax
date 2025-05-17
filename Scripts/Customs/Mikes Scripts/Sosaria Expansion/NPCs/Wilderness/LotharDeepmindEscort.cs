using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LotharDeepmindQuest : BaseQuest
    {
        public override object Title { get { return "Out of the Deep"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Lothar's eyes flicker with a dim, bioluminescent glow. His voice trembles, as though still underwater.*\n\n" +
                    "\"I am Lothar Deepmind... I went too far into the fungal abyss. The spores—they cling to thoughts. If I don’t return soon, I’ll be part of it forever. The tunnels twist, the air hums with voices. Please, lead me back, before my mind sinks for good.\"";
            }
        }

        public override object Refuse { get { return "*Lothar shudders, gripping his head.* \"Then I’ll drift into the dark... alone.\""; } }
        public override object Uncomplete { get { return "*He sways slightly, murmuring.* \"The spores... they itch behind my eyes... hurry...\""; } }

        public LotharDeepmindQuest() : base()
        {
            AddObjective(new EscortObjective("the Perinia Depths Dungeon"));
            AddReward(new BaseReward(typeof(FurTradersChest), "FurTradersChest – A secure container that increases loot yield from animals."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Lothar breathes deeply, his mind clearing as he bows.* \"Thank you... I feel the sea’s pull again. Take this, may it help you gather the bounty of the wild.\"");
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

    public class LotharDeepmindEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(LotharDeepmindQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFurtrader());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public LotharDeepmindEscort() : base()
        {
            Name = "Lothar Deepmind";
            Title = "the Abyssal Explorer";
            NameHue = 2065; // Deep sea blue hue
        }

		public LotharDeepmindEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 55, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1323; // Slightly pale, with a faint sea-sick tint
            HairItemID = 0x2049; // Short, tousled
            HairHue = 2101; // Dark teal, like deep water
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 2101;
        }

        public override void InitOutfit()
        {
            AddItem(new Cloak() { Hue = 1370, Name = "Shroud of the Abyss" }); // Dark green, oceanic
            AddItem(new LeatherGloves() { Hue = 2106, Name = "Grasping Vines" }); // Sickly green
            AddItem(new StuddedLegs() { Hue = 2125, Name = "Spore-Hardened Greaves" }); // Pale fungus color
            AddItem(new Tunic() { Hue = 1373, Name = "Barnacle-Encrusted Jerkin" }); // Mottled blue-green
            AddItem(new Boots() { Hue = 2401, Name = "Trench Boots" }); // Deep muddy hue
            AddItem(new Bandana() { Hue = 1371, Name = "Tide-Bound Wrap" }); // Ocean blue

            AddItem(new GnarledStaff() { Hue = 1260, Name = "Coral Branch" }); // Pinkish coral color

            Backpack backpack = new Backpack();
            backpack.Hue = 2106;
            backpack.Name = "Deepmind's Salvage Pack";
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
                        "*Lothar mutters.* The walls breathe... did you see that?",
                        "*He clutches his head.* The hum... it’s louder now...",
                        "*Lothar gasps.* Don’t let the spores in, don’t breathe too deep!",
                        "*He whispers.* I saw the abyss, and it saw me.",
                        "*His eyes widen.* I almost became... one of them.",
                        "*He laughs nervously.* The ocean feels so far away down here...",
                        "*Lothar stares blankly.* Did you hear them? They know we’re leaving..."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 30));
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
