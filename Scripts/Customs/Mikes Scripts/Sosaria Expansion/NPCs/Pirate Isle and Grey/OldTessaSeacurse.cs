using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BearOfTheDeepQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bear of the Deep"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Old Tessa Seacurse*, survivor of storms and wrecks.\n\n" +
                    "Her eyes are clouded, yet burn with a sharpness born of terror and grit.\n\n" +
                    "“Ye heard the stories of the **SpectralUrsus**? It ain’t no tale to scare children. I saw it, clawin’ through the hull like it was paper. Took my crew, nearly took me too.”\n\n" +
                    "“It lives in the bones of the sea, down by the wrecks near **Exodus Dungeon**. Now it stalks any fool enough to dive there.”\n\n" +
                    "“I ain’t askin’ for pity. I want vengeance. That bear’s a curse on the waves. Kill it, and I’ll see ye get more than just thanks. I can even guide ye close, if ye dare.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then stay dry, sailor. But know this: as long as that beast roams, no wreck’s safe from its claws—or your own shadow from its teeth.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathin’? Then the **SpectralUrsus** still feeds. Don’t let it keep its fangs much longer, aye?";
            }
        }

        public override object Complete
        {
            get
            {
                return "**Gone**, is it? Aye, I feel it. The sea’s quieter now, less hateful.\n\n" +
                       "Ye’ve done me a great turn, and the sea herself, maybe. Take this chest—it’s more than gold, it’s *tactics* to keep ye alive when the deep turns foul.";
            }
        }

        public BearOfTheDeepQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpectralUrsus), "SpectralUrsus", 1));
            AddReward(new BaseReward(typeof(TacticsBonusChest), 1, "TacticsBonusChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bear of the Deep'!");
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

    public class OldTessaSeacurse : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BearOfTheDeepQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith()); // Old sailor’s gear
        }

        [Constructable]
        public OldTessaSeacurse()
            : base("the Shipwreck Survivor", "Old Tessa Seacurse")
        {
        }

        public OldTessaSeacurse(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 65, 40);

            Female = true;
            Body = 0x191; // Female body
            Race = Race.Human;

            Hue = 1002; // Weathered, sea-worn skin
            HairItemID = 8252; // Messy bun
            HairHue = 1150; // Salt-and-pepper grey
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1108, Name = "Storm-Worn Robe" });
            AddItem(new ThighBoots() { Hue = 1109, Name = "Salt-Soaked Boots" });
            AddItem(new Bandana() { Hue = 1110, Name = "Sea-Survivor's Bandana" });
            AddItem(new HalfApron() { Hue = 2101, Name = "Blood-Streaked Apron" });
            AddItem(new ShepherdsCrook() { Hue = 0, Name = "Wreckwood Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Survivor's Satchel";
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
