using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DarnokTheCursedQuest : BaseQuest
    {
        public override object Title { get { return "Bound By Shadows"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Darnok’s hollow eyes lock with yours, flickering with desperate hope.*\n\n" +
                    "\"I am Darnok, once a knight of light, now cursed to roam this pit of despair. My soul is tethered to this forsaken place, inching ever closer to oblivion. Help me escape Doom Dungeon’s grasp before I am lost to the dark forever. I know the path, but my strength wanes. Will you bear this burden with me?\"";
            }
        }

        public override object Refuse { get { return "*Darnok lowers his head, shadows crawling up his armor.* \"Then I remain a prisoner...\""; } }
        public override object Uncomplete { get { return "*His voice echoes with urgency.* \"The curse gnaws... We must press on!\""; } }

        public DarnokTheCursedQuest() : base()
        {
            AddObjective(new EscortObjective("the Doom Dungeon"));
            AddReward(new BaseReward(typeof(PumpkinKingsCrown), "PumpkinKingsCrown – Grants fear immunity and minor necromancy bonuses."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Darnok kneels, his chains dissolving into mist.* \"I am free... and you have my eternal thanks. Take this, may it shield you from dread as it once shielded me from death.\"", null, 0x59B);
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

    public class DarnokTheCursedEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(DarnokTheCursedQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public DarnokTheCursedEscort() : base()
        {
            Name = "Darnok the Cursed";
            Title = "Knight of the Doomed Order";
            NameHue = 0x455;
        }

		public DarnokTheCursedEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 75, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Deathly pale
            HairItemID = 0x2048; // Messy hair
            HairHue = 1150; // Ghostly white
            FacialHairItemID = 0x203F; // Full beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 2101, Name = "Helm of Eternal Vigil" }); // Dark steel
            AddItem(new BoneChest() { Hue = 1175, Name = "Warden's Husk" }); // Faded bone white
            AddItem(new StuddedArms() { Hue = 1175, Name = "Bindings of the Damned" }); // Matching the chest
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Grasp of the Forgotten" }); // Charcoal black
            AddItem(new StuddedLegs() { Hue = 1175, Name = "Leggings of Lost Honor" }); // Worn but resilient
            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of Bound Shadows" }); // Midnight blue with dark sigils
            AddItem(new Boots() { Hue = 2105, Name = "Gravewalker's Stride" }); // Deep obsidian

            AddItem(new WarMace() { Hue = 1175, Name = "Soulhammer" }); // Faintly glowing runes
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Cursed Relic Satchel";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.12)
                {
                    string[] lines = new string[]
                    {
                        "*Darnok clutches his head.* The whispers... they never cease...",
                        "*His eyes flare briefly.* We must hurry, or I am doomed to remain!",
                        "*A chain rattles faintly.* Each step away tears the curse... but the pain...",
                        "*Darnok glances behind.* Something follows us... it wants me back.",
                        "*His voice is hollow.* Doom Dungeon is alive. It remembers us.",
                        "*Darnok breathes heavily.* If I falter, leave me. I will not drag you to the abyss.",
                        "*He grips his mace tightly.* This weapon once defended light. Now, it breaks chains."
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
