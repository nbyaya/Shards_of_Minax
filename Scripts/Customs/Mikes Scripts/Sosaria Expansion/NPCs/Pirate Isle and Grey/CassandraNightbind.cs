using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FelinePestilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Feline Pestilence"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet **Cassandra Nightbind**, the enigmatic Plague Doctor of Grey.\n\n" +
                    "Clad in a dark coat, her face obscured by a birdlike mask shimmering faintly with protective enchantments, she gazes through you rather than at you.\n\n" +
                    "\"Disease moves in shadows, silent but savage. Lately, our dead bear marks not of blade or tooth, but claw—*feline*, infected, unnatural.\"\n\n" +
                    "\"A creature, once cat, now plague incarnate, stalks the Exodus Dungeon. Its breath is rot, its claws a harbinger. **PlagueboundFeline**, they now call it.\"\n\n" +
                    "\"Slay this abomination before it spreads its blight to every isle. And bring me proof, so I might sanctify the remains.\"\n\n" +
                    "**Hunt the PlagueboundFeline in the Exodus Dungeon and stop the spread of disease.**";
            }
        }

        public override object Refuse
        {
            get { return "\"Then we are doomed to rot beneath fur and claw. I pray the Isles forgive your silence.\""; }
        }

        public override object Uncomplete
        {
            get { return "\"It yet lives? The pestilence grows bolder. Hurry, before the winds of Grey carry it further.\""; }
        }

        public override object Complete
        {
            get
            {
                return
                    "\"It is done? The feline’s plague breath snuffed at last... Good. You’ve saved more than you know.\"\n\n" +
                    "\"Take this, a **Hildebrandt Tapestry**, woven with threads meant to ward disease. Hang it where sickness dares not tread.\"";
            }
        }

        public FelinePestilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PlagueboundFeline), "PlagueboundFeline", 1));
            AddReward(new BaseReward(typeof(HildebrandtTapestry), 1, "Hildebrandt Tapestry"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Feline Pestilence'!");
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

    public class CassandraNightbind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FelinePestilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public CassandraNightbind()
            : base("the Plague Doctor", "Cassandra Nightbind")
        {
        }

        public CassandraNightbind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale
            HairItemID = 0x203C; // Long Hair
            HairHue = 1109; // Black
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Plagueweaver’s Shroud" });
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Sanitizer’s Touch" });
            AddItem(new Boots() { Hue = 2101, Name = "Treader of Shadows" });
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Doctor’s Beaked Mask" });

            AddItem(new QuarterStaff() { Hue = 1175, Name = "Rod of Purging" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbalist's Satchel";
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
