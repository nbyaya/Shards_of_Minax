using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SerpentsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Serpent’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Caelia Coilweaver*, the castle’s enigmatic enchanter, robed in silken garments interwoven with fine metallic threads that seem to hum softly.\n\n" +
                    "Her eyes spark with magnetic intensity as she leans over a table scattered with glowing runes and strange coils.\n\n" +
                    "“You must listen. One of my earlier enchantments has turned against us.”\n\n" +
                    "“Deep within Preservation Vault 44, a creature—a *SyntheticOphidian*—has wound itself into the vault’s heart, twisting my coil enchantments to snare all who enter. It has fused metal and magic into a labyrinthine prison, and many who sought its secrets are lost.”\n\n" +
                    "“But I have forged a disruptor. It can fracture its scales, if you can strike true.”\n\n" +
                    "**Go to Preservation Vault 44. End the SyntheticOphidian. End the corruption of my craft.**”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the Vault. Its coils tighten. And soon, they will reach beyond its walls.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it coils and crushes? My disruptor hums impatiently... and my shame grows heavier.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The SyntheticOphidian is no more? *You have uncoiled the serpent and freed the vault.*\n\n" +
                       "Take this: **TheAdaptix**. Born from the same magnetic arts that forged the disruptor—it bends, shifts, and flows to match your will.\n\n" +
                       "May it serve you as well as you have served Sosaria.";
            }
        }

        public SerpentsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SyntheticOphidian), "SyntheticOphidian", 1));
            AddReward(new BaseReward(typeof(TheAdaptix), 1, "TheAdaptix"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Serpent’s End'!");
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

    public class CaeliaCoilweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SerpentsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); // Closest match to her enchanter profession
        }

        [Constructable]
        public CaeliaCoilweaver()
            : base("the Coil Enchanter", "Caelia Coilweaver")
        {
        }

        public CaeliaCoilweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale magical tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1150; // Silver-blue hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1152, Name = "Magnetite Robe" }); // Shimmering cobalt
            AddItem(new BodySash() { Hue = 2101, Name = "Coilbinder's Sash" }); // Deep violet
            AddItem(new WizardsHat() { Hue = 2075, Name = "Helm of Flux" }); // Electric indigo
            AddItem(new Sandals() { Hue = 1154, Name = "Arcane Treaders" }); // Light blue shimmer
            AddItem(new GnarledStaff() { Hue = 1161, Name = "Disruptor's Wand" }); // Magnetic silver

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Enchanter's Pack";
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
