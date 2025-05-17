using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RurikStonefistQuest : BaseQuest
    {
        public override object Title { get { return "Stonefist's Last Stand"; } }

        public override object Description
        {
            get
            {
                return
                    "*The dwarf’s eyes burn with a fierce resolve beneath a weather-worn helm.*\n\n" +
                    "I am Rurik Stonefist, shield-bearer of the Mountain Stronghold. My kin are in grave danger—an avalanche of enemies descends upon them even now. I must return to warn them, to fight if I must. But the roads are treacherous, and I can’t make it alone. Will you guide me to the Stronghold before it’s too late?";
            }
        }

        public override object Refuse { get { return "*Rurik tightens his grip on his hammer.* Then I pray the stones will guide me alone..."; } }
        public override object Uncomplete { get { return "*Rurik glowers.* We tarry too long! My kin need me!"; } }

        public RurikStonefistQuest() : base()
        {
            AddObjective(new EscortObjective("a Mountain Stronghold"));
            AddReward(new BaseReward(typeof(TechnicolorTalesChest), "TechnicolorTalesChest – A container filled with brightly enchanted scrolls and mysterious items."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Rurik clasps your hand with surprising strength.* You’ve earned the thanks of the Stonefist clan. Take this chest—its wonders are many, and well-deserved. May your path be ever lit by the fires of courage.", null, 0x59B);
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

    public class RurikStonefistEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(RurikStonefistQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith()); // Reflects his role as a dwarven warrior and armorer.
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public RurikStonefistEscort() : base()
        {
            Name = "Rurik Stonefist";
            Title = "the Shield-Bearer";
            NameHue = 0x501;
        }

		public RurikStonefistEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(85, 60, 50);

            Female = false;
            Body = 0x190; // Male


            Hue = 1103; // Ruddy, earth-toned skin.
            HairItemID = 0x2044; // Long, braided beard.
            HairHue = 1813; // Fiery auburn.
            FacialHairItemID = 0x203F; // Full, braided beard.
            FacialHairHue = 1813;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 2425, Name = "Stonefist Helm" }); // Earthy bronze.
            AddItem(new PlateChest() { Hue = 2415, Name = "Runeforged Cuirass" }); // Deep iron-grey with glowing runes.
            AddItem(new PlateArms() { Hue = 2425, Name = "Mountain Guard Bracers" });
            AddItem(new PlateGloves() { Hue = 2413, Name = "Fistwrap Gauntlets" });
            AddItem(new PlateLegs() { Hue = 2415, Name = "Stonebound Greaves" });
            AddItem(new Cloak() { Hue = 1157, Name = "Cloak of the Emberdeep" }); // A rich crimson cloak, marked with clan symbols.
            AddItem(new Boots() { Hue = 2419, Name = "Granite-Tread Boots" });

            AddItem(new WarHammer() { Hue = 2406, Name = "Stonefist's Oathbreaker" }); // Heavy, rune-inscribed warhammer.

            Backpack backpack = new Backpack();
            backpack.Hue = 1108;
            backpack.Name = "Stonefist's Pack";
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
                        "*Rurik grunts.* The mountain calls to me—we must make haste!",
                        "*His eyes narrow.* I’ll not see my kinfall in silence!",
                        "*Rurik clenches his hammer.* Enemies gather like storm clouds... they’ll feel the weight of Stonefist steel.",
                        "*He glances at the sky.* These lands were once ours. Now, we reclaim them, or die in the trying.",
                        "*The dwarf mutters.* The Stronghold stands, but for how long?",
                        "*Rurik growls softly.* Stay sharp—the paths are never as empty as they seem.",
                        "*His voice softens.* My brother fell here, years past... I won’t let his fate be in vain."
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
