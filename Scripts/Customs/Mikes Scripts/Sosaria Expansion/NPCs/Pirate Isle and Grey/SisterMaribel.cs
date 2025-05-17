using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WatchersFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Watcher’s Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you kneels *Sister Maribel*, garbed in robes that shimmer with the hues of dusk and dawn.\n\n" +
                    "Her voice is soft, yet echoes with unshaken faith:\n\n" +
                    "“The **Eternal Watcher**... once a guardian of the crypt, now a curse upon us. For centuries, it stood vigil, unmoving, unyielding, until the night of the broken chant.”\n\n" +
                    "“The chapel's crypt in **Exodus Dungeon** trembles with its rage. Each night, the people of Grey hear the Watcher's lament—**a chant of sorrow and judgment**.”\n\n" +
                    "“This is not merely a monster to slay, but a soul to release. Its bond to our world is pain, its strength fueled by forgotten rites.”\n\n" +
                    "**Slay the Eternal Watcher**, and perhaps, at last, our nights will be silent once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we shall pray harder, though each night brings more voices to the chant.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it watches? The night groans with its sorrow, and I fear the dawn may never come.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The chants have ceased... and the crypt is at peace.\n\n" +
                       "You have done more than silence a monster—you have ended an era of dread.\n\n" +
                       "Take these: **Courtesan’s Whispering Gloves**. They were once worn by those who knew how to still the hearts of many, as you have stilled the sorrow of one.";
            }
        }

        public WatchersFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EternalWatcher), "Eternal Watcher", 1));
            AddReward(new BaseReward(typeof(CourtesansWhisperingGloves), 1, "Courtesan’s Whispering Gloves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Watcher’s Fall'!");
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

    public class SisterMaribel : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WatchersFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMonk());
        }

        [Constructable]
        public SisterMaribel()
            : base("Sister of the Eternal Vigil", "Sister Maribel")
        {
        }

        public SisterMaribel(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, ethereal
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Moonlit silver
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2213, Name = "Robe of the Eternal Vigil" }); // Midnight blue
            AddItem(new Sandals() { Hue = 2101, Name = "Sister's Silent Steps" }); // Deep violet
            AddItem(new BodySash() { Hue = 2413, Name = "Sash of Grey's Dusk" }); // Smoky grey
            AddItem(new HoodedShroudOfShadows() { Hue = 2106, Name = "Veil of Vigilance" }); // Dim lavender

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Prayer Satchel";
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
