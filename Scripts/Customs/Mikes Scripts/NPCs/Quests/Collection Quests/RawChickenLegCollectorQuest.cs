using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RawChickenLegCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Craving for Chicken Legs"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Grunthor the Glutton, and I am in dire need of your help. " +
                       "My famed feast is missing one crucial ingredient: 50 RawChickenLegs. " +
                       "These legs are not just for the banquet; they hold the secret to an ancient recipe passed down through generations. " +
                       "In exchange for your assistance, I shall reward you with gold, a rare Maxxia Scroll, and a custom-made Glutton's Attire. " +
                       "Help me restore this sacred culinary tradition!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the RawChickenLegs."; } }

        public override object Uncomplete { get { return "I still need 50 RawChickenLegs. Fetch them for me, and the ancient recipe shall be yours!"; } }

        public override object Complete { get { return "Excellent work, adventurer! You have gathered the 50 RawChickenLegs I required. Your dedication is commendable. " +
                       "As a token of my gratitude, please accept these rewards. May your future feasts be as grand as this one!"; } }

        public RawChickenLegCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RawChickenLeg), "RawChickenLegs", 50, 0x1607)); // Assuming RawChickenLeg item ID is 0x13F8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CrushersGauntlets), 1, "Glutton's Attire")); // Assuming Glutton's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Craving for Chicken Legs quest!");
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

    public class GrunthorTheGlutton : MondainQuester
    {
        [Constructable]
        public GrunthorTheGlutton()
            : base("The Glutton", "Grunthor")
        {
        }

        public GrunthorTheGlutton(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(150, 100, 50);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Grunthor's Feast Armor" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Grunthor's Jester Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Grunthor's Golden Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grunthor's Banquet Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RawChickenLegCollectorQuest)
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
