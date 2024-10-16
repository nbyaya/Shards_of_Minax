using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LavaSerpenCrustCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fiery Crust Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Ignis, the Flame Warden. " +
                       "The Lava Serpents that dwell in the fiery depths have shed their crusts, which are vital for my alchemical experiments. " +
                       "If you could bring me 50 Lava Serpen Crusts, I shall reward you handsomely. " +
                       "In addition to gold, I will present you with a rare Maxxia Scroll and a piece of attire imbued with the essence of the volcanic depths.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Lava Serpen Crusts."; } }

        public override object Uncomplete { get { return "I still require 50 Lava Serpen Crusts. Please bring them to me so that my experiments may continue!"; } }

        public override object Complete { get { return "Fantastic! You have retrieved the 50 Lava Serpen Crusts I needed. " +
                       "Your assistance is invaluable. Accept these rewards as a token of my gratitude, and may the fires of the volcano guide you!"; } }

        public LavaSerpenCrustCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(LavaSerpentCrust), "Lava Serpen Crusts", 50, 0x572D)); // Assuming Lava Serpen Crust item ID is 0xF7F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NecromancersCape), 1, "Flame Warden's Cape")); // Assuming Flame Warden's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fiery Crust Collector quest!");
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

    public class FlameWardenIgnis : MondainQuester
    {
        [Constructable]
        public FlameWardenIgnis()
            : base("The Flame Warden", "Ignis")
        {
        }

        public FlameWardenIgnis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Ignis's Fiery Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Ignis's Flame Cap" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Ignis's Lava Gloves" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Ignis's Ember Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ignis's Inferno Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LavaSerpenCrustCollectorQuest)
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
