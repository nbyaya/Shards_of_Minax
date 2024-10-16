using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceCCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence Collector's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elara, the Essence Collector. I need your assistance to gather 50 EssenceControl, " +
                       "which are crucial for my research. Your efforts will be rewarded with gold, a rare Maxxia Scroll, and a unique " +
                       "Essence Collector's Garb that will signify your contribution.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the EssenceControls."; } }

        public override object Uncomplete { get { return "I still need 50 EssenceControls. Please bring them to me so that I can continue my work!"; } }

        public override object Complete { get { return "Excellent work! You have brought me the 50 EssenceControls I required. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your future endeavors be successful!"; } }

        public EssenceCCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceControl), "EssenceControl", 50, 0x571C)); // Assuming EssenceControl item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MoltenShapersShield), 1, "Essence Collector's Garb")); // Assuming Essence Collector's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence Collector's Challenge quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class EssenceCollectorElara : MondainQuester
    {
        [Constructable]
        public EssenceCollectorElara()
            : base("The Essence Collector", "Elara")
        {
        }

        public EssenceCollectorElara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elara's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Elara's Enchanted Hat" });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Elara's Spellbook" }); // Assuming Spellbook is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elara's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceCCollectorQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
