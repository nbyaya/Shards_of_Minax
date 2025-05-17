using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FabricOfFearQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fabric of Fear"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Helena Stitchwick*, Dawn’s renowned tailor, her hands trembling as she fingers a scorched bolt of silk.\n\n" +
                    "“This cloth—this **was** a gown for Lady Edda of Yew. But now? Look! Torn as if by claws, and these **burn scars**, not from fire, but something *arcane*.”\n\n" +
                    "“It haunts my dreams, this creature. A hulking shadow, loping through the night, shredding every shipment from the southern roads. My courier—he saw it. Said it *moved on all fours*, like a beast but worse.”\n\n" +
                    "“I’m just a tailor. I stitch, I mend. But this... this *Cult Monster* from Doom Dungeon—it’s ruining me, and worse, it’s marked my work. I fear for more than fabric now.”\n\n" +
                    "**Slay the Cult Monster** that ravages my shipments, and bring peace to my looms. Only then can Dawn's warmth be restored.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I pray my thread holds. But fear twists more than fabric—it tears at souls.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? Then I must warn the others—none are safe if it begins to hunger for more than cloth.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You've done it? The looms feel lighter, the shadows less sharp.\n\n" +
                       "Please, take this—*SongOfTheSleeplessHollow*. It was meant for a hero in a tale. Now it belongs to one.";
            }
        }

        public FabricOfFearQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultMonster), "Cult Monster", 1));
            AddReward(new BaseReward(typeof(SongOfTheSleeplessHollow), 1, "SongOfTheSleeplessHollow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fabric of Fear'!");
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

    public class HelenaStitchwick : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FabricOfFearQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTailor());
        }

        [Constructable]
        public HelenaStitchwick()
            : base("the Threadbare Survivor", "Helena Stitchwick")
        {
        }

        public HelenaStitchwick(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(65, 75, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale skin
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Moonlit silver
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2411, Name = "Twilight Weave Dress" }); // Deep violet
            AddItem(new BodySash() { Hue = 2055, Name = "Sash of the Last Stitch" }); // Midnight blue
            AddItem(new ThighBoots() { Hue = 1109, Name = "Charred Leather Boots" }); // Burnt black
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Tailor's Pride" }); // Frost white

            AddItem(new SewingNeedle() { Hue = 0, Name = "Stitchwick’s Needle" }); // Regular

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bolt of Scorched Silk";
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
