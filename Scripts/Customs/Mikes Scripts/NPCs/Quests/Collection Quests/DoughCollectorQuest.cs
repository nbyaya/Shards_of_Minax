using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DoughCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Baker's Bounty"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Grumble, the master baker. My bakery has been graced by a magical recipe, but I lack " +
                       "the essential ingredient: Dough. I need 500 units of Dough to complete my masterpiece. Your assistance will be rewarded " +
                       "with gold, a rare Maxxia Scroll, and a unique Baker's Apron adorned with enchanted flourishes.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Dough."; } }

        public override object Uncomplete { get { return "I still need 50 units of Dough. Please bring them to me so I can complete my recipe!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Dough I needed. Your help has ensured that my masterpiece is completed. " +
                       "Accept these rewards as a token of my gratitude. May your path be as sweet as freshly baked bread!"; } }

        public DoughCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Dough), "Dough", 500, 0x103d)); // Assuming Dough item ID is 0x1034
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SawyersMightyApron), 1, "Baker's Apron")); // Assuming Baker's Apron is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Baker's Bounty quest!");
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

    public class MasterBakerGrumble : MondainQuester
    {
        [Constructable]
        public MasterBakerGrumble()
            : base("The Master Baker", "Grumble")
        {
        }

        public MasterBakerGrumble(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Grumble's Baker's Apron" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new TricorneHat { Hue = Utility.Random(1, 3000), Name = "Grumble's Chef's Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Grumble's Baker's Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grumble's Baking Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DoughCollectorQuest)
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
