using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DeadWoodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Alderic's Collection Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Alderic, the Collector. I am in dire need of 50 pieces of DeadWood. " +
                       "These ancient woods are crucial for my magical concoctions. Your help in gathering them would be " +
                       "immensely appreciated. In return, I will reward you with gold, a rare Maxxia Scroll, and a set of enchanted " +
                       "gear that will surely catch the eye of all who see you.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, come back with the DeadWood."; } }

        public override object Uncomplete { get { return "I still need 50 pieces of DeadWood. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 pieces of DeadWood. Your assistance is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May your journey be ever fruitful!"; } }

        public DeadWoodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DeadWood), "DeadWood", 50, 0xF90)); // Assuming DeadWood item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GrandmastersRobe), 1, "Enchanted Robe")); // Assuming Enchanted Robe is a custom item
            AddReward(new BaseReward(typeof(MysticsFeatheredHat), 1, "Enchanted Hat")); // Assuming Enchanted Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Alderic's Collection Challenge quest!");
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

    public class AldericTheCollector : MondainQuester
    {
        [Constructable]
        public AldericTheCollector()
            : base("Alderic the Collector", "Alderic")
        {
        }

        public AldericTheCollector(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Alderic's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Alderic's Mystical Hat" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Alderic's Arcane Staff" }); // Assuming Arcane Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Alderic's Collector's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DeadWoodCollectorQuest)
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
