using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class InspectorFenwickQuest : BaseQuest
    {
        public override object Title { get { return "Claws of Conspiracy"; } }

        public override object Description
        {
            get
            {
                return
                    "*Inspector Fenwick adjusts his crimson coat, eyes sharp beneath a furrowed brow.*\n\n" +
                    "\"I am Fenwick, Inspector of the Crown. I've uncovered threads of a conspiracy, one that stretches into the lair of the last Firstborn Dragon, Drakkon. My mission is clear: enter those cursed caves, gather proof, and expose the traitors. Yet, the path is perilous, and my foes seek to silence me. Will you escort me, so truth may burn brighter than any dragonfire?\"";
            }
        }

        public override object Refuse { get { return "*Fenwick taps his walking cane, a grim smile curling.* \"Then I go alone, and may justice still find its mark.\""; } }
        public override object Uncomplete { get { return "*Fenwick glances toward the cavern's depths.* \"Stay vigilant—danger coils tighter with every step.\""; } }

        public InspectorFenwickQuest() : base()
        {
            AddObjective(new EscortObjective("the Clues Dungeon"));
            AddReward(new BaseReward(typeof(LuchadorsMask), "LuchadorsMask – Increases grappling power and charisma."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Fenwick removes his mask and hands it to you solemnly.* \"You've seen the shadows I walk. Take this, a symbol of strength and persuasion. The truth lives because of you.\"", null, 0x59B);
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

    public class InspectorFenwickEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(InspectorFenwickQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVarietyDealer()); // Closest to an investigator/vagabond
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public InspectorFenwickEscort() : base()
        {
            Name = "Inspector Fenwick";
            Title = "the Inquisitor";
            NameHue = 1157; // Dark red for authority and intensity
        }

		public InspectorFenwickEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 70, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered complexion
            HairItemID = 0x203C; // Short hair
            HairHue = 1109; // Dark brown
            FacialHairItemID = 0x204B; // Goatee
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet() { Hue = 1157, Name = "Fenwick's Crimson Coat" }); // Blood red
            AddItem(new LongPants() { Hue = 1109, Name = "Investigator's Trousers" }); // Charcoal
            AddItem(new ThighBoots() { Hue = 1175, Name = "Pathfinder's Boots" }); // Dark leather
            AddItem(new Cloak() { Hue = 1171, Name = "Cloak of Vigilance" }); // Deep burgundy
            AddItem(new WideBrimHat() { Hue = 1150, Name = "Shadowed Brim Hat" }); // Matte black
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Grip of Justice" }); // Ash grey
            AddItem(new DetectivesBoneHarvester() { Hue = 1157, Name = "Truthseeker's Cane" }); // Red-tinted weapon cane

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Inspector's Satchel";
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
                        "*Fenwick scans the shadows.* Every step draws us closer to the truth... or death.",
                        "*He grips his cane tightly.* Drakkon hides more than gold. There are secrets... treacherous ones.",
                        "*Fenwick nods to you.* Keep your senses sharp—the air thickens with deceit.",
                        "*He adjusts his coat.* I’ve faced men and monsters, but never a dragon’s court.",
                        "*A glint in his eye.* Exposing them is worth the risk. The realm must know.",
                        "*He smirks.* They thought they'd silence me. Let’s show them otherwise.",
                        "*He whispers.* I trust you. Few I can say that about these days."
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
