using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PagesOfPerilQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Pages of Peril"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Lyra Silverscribe*, Keeper of the Forbidden Stacks in Death Glutch’s haunted library.\n\n" +
                    "She holds a tome, bound in blackened silver, its pages fluttering as if in a breeze only she can feel.\n\n" +
                    "“My life’s work… threatened by a curse made flesh.”\n\n" +
                    "“One of the forbidden tomes was *tampered with*. I suspect foul play—another scholar seeking to discredit me or unlock powers best left buried.”\n\n" +
                    "**The Enchanted Minion**, a creature born of ink and malice, now haunts the archives of the **Malidor Witches Academy**. It animates the very books meant to be studied, twisting their knowledge into chaos.”\n\n" +
                    "“I cannot bear to see centuries of wisdom destroyed or corrupted. Slay this monstrosity, and help me preserve the fragile truths we guard.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the ink drown us all. But know this—when the pages turn against us, it will be your silence that wrote our doom.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lingers? I can feel the books resisting me, whispering lies. Every moment we delay, the Minion grows stronger.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done more than slay a creature—you’ve safeguarded **knowledge itself**.\n\n" +
                       "The tomes are still scarred, but we will recover. Take this: *EdgarsEngineerChainmail*, once worn by a scholar-warrior like yourself. May it protect you in battles yet to come.";
            }
        }

        public PagesOfPerilQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EnchantedMinion), "Enchanted Minion", 1));
            AddReward(new BaseReward(typeof(EdgarsEngineerChainmail), 1, "EdgarsEngineerChainmail"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Pages of Peril'!");
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

    public class LyraSilverscribe : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PagesOfPerilQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public LyraSilverscribe()
            : base("the Keeper of the Forbidden Stacks", "Lyra Silverscribe")
        {
        }

        public LyraSilverscribe(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Icy Silver
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1109, Name = "Shadowweave Robe" }); // Deep midnight blue
            AddItem(new Cloak() { Hue = 1157, Name = "Ink-Touched Cloak" }); // Glossy black
            AddItem(new Sandals() { Hue = 1175, Name = "Moonshadow Sandals" }); // Dark grey with silver trim
            AddItem(new SkullCap() { Hue = 1153, Name = "Scholar’s Circlet" }); // Pale silver
            AddItem(new BodySash() { Hue = 1171, Name = "Glyph-Marked Sash" }); // Dark purple

            AddItem(new ScribeSword() { Hue = 2101, Name = "Tomecutter" }); // Blackened blade, used for cutting pages

            Backpack backpack = new Backpack();
            backpack.Hue = 1161;
            backpack.Name = "Librarian's Satchel";
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
