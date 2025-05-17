using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DepthsOfDespairQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Depths of Despair"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Mira Emberlight*, Archivist of Devil Guard, surrounded by parchments and flickering lanterns.\n\n" +
                    "Her fingers trace ancient symbols on a tattered map, her voice steady yet tinged with urgency:\n\n" +
                    "“Beneath our feet, the past stirs. The *DeepOrtanord*, a creature bound to ancient knowledge, has awakened in the Mines of Minax.”\n\n" +
                    "“I have traced its presence to lost archives of the Planar Imperium. I must have that tome it guards—within it lie excavation techniques forgotten by time.”\n\n" +
                    "“Will you brave the depths? Slay the beast, and return with the tome. It is our best hope to reclaim the knowledge buried in darkness.”\n\n" +
                    "**Slay the DeepOrtanord** and bring me its tome, so we may learn what was lost.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Without the tome, our knowledge crumbles like the mines themselves. I hope the darkness does not claim us all.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The DeepOrtanord still prowls the depths? Time is not our ally. The longer it guards the tome, the deeper the truth hides.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have returned, and with it, the lost wisdom of the Imperium.\n\n" +
                       "This tome... I can already feel its weight, not just in hand, but in history.\n\n" +
                       "Take this *StonefireTartanKilt*, woven in honor of miners who dared the abyss. You have earned your place among them.";
            }
        }

        public DepthsOfDespairQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DeepOrtanord), "DeepOrtanord", 1));
            AddReward(new BaseReward(typeof(StonefireTartanKilt), 1, "StonefireTartanKilt"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Depths of Despair'!");
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

    public class MiraEmberlight : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DepthsOfDespairQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public MiraEmberlight()
            : base("the Archivist", "Mira Emberlight")
        {
        }

        public MiraEmberlight(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1023; // Pale tone with slight desert warmth
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Crimson Red
        }

        public override void InitOutfit()
        {
            AddItem(new FormalShirt() { Hue = 1154, Name = "Soot-Kissed Scholar’s Shirt" }); // Deep gray-blue
            AddItem(new FancyKilt() { Hue = 1157, Name = "Emberlight Tartan" }); // Warm amber-red
            AddItem(new LeatherGloves() { Hue = 2425, Name = "Scripted Gauntlets" }); // Inscribed with glyphs
            AddItem(new BodySash() { Hue = 1175, Name = "Archivist’s Sash" }); // Midnight black
            AddItem(new Boots() { Hue = 2101, Name = "Ashen Leather Boots" }); // Volcanic black
            AddItem(new FeatheredHat() { Hue = 1171, Name = "Scholar’s Plume" }); // Dark red with a raven feather
            AddItem(new SpellWeaversWand() { Hue = 1161, Name = "Lanternstaff of Echoes" }); // Glows faintly

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Emberlight's Research Pack";
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
