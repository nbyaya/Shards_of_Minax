using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EcruCitrineCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Ecru Citrine Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Orin, the Keeper of the Lost Enclave. I have been searching for 50 Ecru Citrines, " +
                       "gemstones of great mystical power that hold the key to restoring the ancient wards protecting our realm. " +
                       "Your assistance in gathering these gems will be rewarded with a generous sum of gold, a rare Maxxia Scroll, " +
                       "and a magical garb infused with the essence of the lost enclave. Will you aid me in this crucial task?";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Ecru Citrines."; } }

        public override object Uncomplete { get { return "I still require 50 Ecru Citrines to complete the restoration. Please bring them to me!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Ecru Citrines I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May you always find fortune in your journeys!"; } }

        public EcruCitrineCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EcruCitrine), "Ecru Citrines", 50, 0x3195)); // Assuming Ecru Citrine item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CarpentersStalwartTunic), 1, "Lost Enclave's Garb")); // Assuming Lost Enclave's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Ecru Citrine Collector quest!");
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

    public class KeeperOrin : MondainQuester
    {
        [Constructable]
        public KeeperOrin()
            : base("The Keeper of the Lost Enclave", "Orin")
        {
        }

        public KeeperOrin(Serial serial)
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
            AddItem(new FemalePlateChest { Hue = Utility.Random(1, 3000), Name = "Orin's Enclave Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Orin's Enchanted Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Orin's Mystical Gloves" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Orin's Ancient Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Orin's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EcruCitrineCollectorQuest)
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
