using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExperimentX17Quest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Experiment X-17"; } }

        public override object Description
        {
            get
            {
                return
                    "You step into a cluttered, smoke-swirled lab nestled within the scorched remains of Death Glutch. **Dr. Felwyn Cerebro**, alchemical visionary—or pariah, depending on who you ask—greets you with wild eyes and ink-stained gloves.\n\n" +
                    "“Ah, perfect timing! I require assistance of the *lethal* variety.”\n\n" +
                    "“One of my creations, *LabRat*, has... accelerated beyond all projections. It escaped during the last storm, scurried through the wards, and now nests in the decaying halls of **Malidor Witches Academy**.”\n\n" +
                    "“It is imperative you eliminate Experiment X-17. It's not just dangerous—it’s *learning*. And I fear it may attempt to replicate itself.”\n\n" +
                    "“I promise you, no more of that serum will be crafted. Help me clean this mistake, and I’ll share with you something from my own mind.”\n\n" +
                    "**Destroy LabRat**, and ensure no trace of its serum remains.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Refuse if you must—but know, each hour that creature evolves. If I fall to it... well, I shall haunt you for eternity!”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still alive? Excellent. But *so is it*. Until you end it, my conscience won’t be the only thing unraveling.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“You’ve done it? You’ve truly destroyed X-17?”\n\n" +
                       "*Felwyn breathes deeply, then grins—half-relieved, half-inspired.*\n\n" +
                       "“Marvelous. You’ve not only saved me—you’ve spared the world my worst impulse. Here, take this: *TheThinkingCap*. It’s said to focus the mind, though I’ve yet to wear it without hearing voices.”";
            }
        }

        public ExperimentX17Quest() : base()
        {
            AddObjective(new SlayObjective(typeof(LabRat), "LabRat", 1));
            AddReward(new BaseReward(typeof(TheThinkingCap), 1, "TheThinkingCap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Experiment X-17'!");
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

    public class DrFelwynCerebro : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExperimentX17Quest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public DrFelwynCerebro()
            : base("the Alchemical Researcher", "Dr. Felwyn Cerebro")
        {
        }

        public DrFelwynCerebro(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale with an otherworldly tone
            HairItemID = 0x2047; // Long Hair
            HairHue = 1150; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1175, Name = "Void-Touched Robe" }); // Deep violet
            AddItem(new HalfApron() { Hue = 1150, Name = "Serum-Stained Apron" }); // Muted blue-gray
            AddItem(new WizardsHat() { Hue = 1266, Name = "Cerebral Conductor" }); // Pale silver
            AddItem(new Sandals() { Hue = 1157, Name = "Soft-Step Sandals" }); // Charcoal
            AddItem(new LeatherGloves() { Hue = 1170, Name = "Alchemist's Gauntlets" }); // Slate-blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1154; // Arcane blue
            backpack.Name = "Satchel of Unfinished Theories";
            AddItem(backpack);

            AddItem(new Scepter() { Hue = 1171, Name = "Focus of Volatile Thoughts" }); // Shimmering blue
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
