using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class JarHoneyCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Honeyed Harvest"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave traveler! I am Brindle, the Honey Sage. My hive's nectar has been stolen by " +
                       "marauding creatures, and now I require 50 jars of honey to restore my collection. The honey is not just a " +
                       "simple sweetener; it holds ancient enchantments that are crucial for my research. Bring me the jars, and " +
                       "I shall reward you handsomely with gold, a rare Maxxia Scroll, and a uniquely enchanted Honey Sage's Kilt.";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, bring me the jars of honey and we shall speak again."; } }

        public override object Uncomplete { get { return "I still require 50 jars of honey. Please bring them to me so I can continue my work!"; } }

        public override object Complete { get { return "Excellent work, adventurer! You have brought me the 50 jars of honey. Your assistance is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May your path be filled with sweetness!"; } }

        public JarHoneyCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(JarHoney), "Jars of Honey", 50, 0x9ec)); // Assuming JarHoney item ID is 0x9F9
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ShepherdsKilt), 1, "Honey Sage's Kilt")); // Assuming Honey Sage's Vestment is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Honeyed Harvest quest!");
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

    public class HoneySageBrindle : MondainQuester
    {
        [Constructable]
        public HoneySageBrindle()
            : base("The Honey Sage", "Brindle")
        {
        }

        public HoneySageBrindle(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Brindle's Honeyed Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherArms { Hue = Utility.Random(1, 3000), Name = "Brindle's Floral Garland" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Brindle's Enchanted Ring" });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Brindle's Sage Hat" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Brindle's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(JarHoneyCollectorQuest)
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
