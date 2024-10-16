using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WondrousFishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Wondrous Fish Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Alaric, the Keeper of the Mystic Waters. " +
                       "Legends speak of a rare breed of fish known as the Wondrous Fish, which possess extraordinary powers. " +
                       "I need you to collect 50 of these magnificent creatures for me. " +
                       "Their scales hold secrets of an ancient magic that must be harnessed to protect our waters. " +
                       "In exchange for your help, you will be rewarded with gold, a rare Maxxia Scroll, and the fabled Keeper's Enchanted Attire.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back with the Wondrous Fish."; } }

        public override object Uncomplete { get { return "I still need 50 Wondrous Fish to complete my research. Please bring them to me."; } }

        public override object Complete { get { return "Splendid! You have gathered all 50 Wondrous Fish. Your dedication to this quest is commendable. " +
                       "Accept these rewards as a token of my gratitude. May the magic of the waters guide you on your future adventures!"; } }

        public WondrousFishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WondrousFish), "Wondrous Fish", 50, 0xDD6)); // Assuming Wondrous Fish item ID is 0xF5F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SamuraisHonorableTunic), 1, "Keeper's Enchanted Attire")); // Assuming Keeper's Enchanted Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Wondrous Fish Collector quest!");
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

    public class KeeperAlaric : MondainQuester
    {
        [Constructable]
        public KeeperAlaric()
            : base("The Keeper of the Mystic Waters", "Alaric")
        {
        }

        public KeeperAlaric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Alaric's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Alaric's Mystic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Alaric's Seer's Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Necklace { Hue = Utility.Random(1, 3000), Name = "Alaric's Magic Amulet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Alaric's Oceanic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WondrousFishCollectorQuest)
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
