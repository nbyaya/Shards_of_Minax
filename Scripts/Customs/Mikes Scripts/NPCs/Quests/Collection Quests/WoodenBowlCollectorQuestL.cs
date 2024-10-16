using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WoodenBowlCollectorQuestL : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Wooden Bowl Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Gormar the Ancient, keeper of the sacred recipes. I require your help to gather 50 Wooden Bowls of Lettuce. " +
                       "These bowls are used in a long-forgotten ceremony to restore balance to the lands. Your efforts will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a unique Gormar's Ceremonial Attire. Your contribution will be remembered through the ages!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Wooden Bowls of Lettuce."; } }

        public override object Uncomplete { get { return "I still need 50 Wooden Bowls of Lettuce. Please bring them to me as soon as you can."; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Wooden Bowls of Lettuce. Your assistance is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May your path be blessed!"; } }

        public WoodenBowlCollectorQuestL() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfLettuce), "Wooden Bowls of Lettuce", 50, 0x15FB)); // Assuming Wooden Bowl of Lettuce item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HarmonicHelm), 1, "Gormar's Ceremonial Attire")); // Assuming Gormar's Ceremonial Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Wooden Bowl Collector quest!");
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

    public class GormarTheAncient : MondainQuester
    {
        [Constructable]
        public GormarTheAncient()
            : base("Gormar the Ancient", "Gormar")
        {
        }

        public GormarTheAncient(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Gormar's Ceremonial Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Gormar's Sacred Crown" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Gormar's Enchanted Cloak" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Gormar's Mystical Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gormar's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodenBowlCollectorQuestL)
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
