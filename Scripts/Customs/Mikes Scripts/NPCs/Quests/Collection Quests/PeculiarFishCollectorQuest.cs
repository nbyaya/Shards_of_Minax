using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class PeculiarFishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Peculiar Fish Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Thalgar, the Keeper of Secrets from the Depths. I require your help to collect 50 Peculiar Fish, " +
                       "mysterious creatures that dwell in the deepest and darkest waters. Their scales shimmer with ancient magic, and they are vital for my studies " +
                       "on the hidden realms of the ocean. In return for your efforts, you shall be rewarded with gold, a rare Maxxia Scroll, and a unique Hat " +
                       "that bears the mark of the ocean's depth.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Peculiar Fish."; } }

        public override object Uncomplete { get { return "I still need 50 Peculiar Fish. Please bring them to me so I can continue my studies."; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Peculiar Fish I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May the tides of fortune be ever in your favor!"; } }

        public PeculiarFishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PeculiarFish), "Peculiar Fish", 50, 0xDD6)); // Assuming Peculiar Fish item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(IntriguersFeatheredHat), 1, "Oceanic Hat")); // Assuming Oceanic Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Peculiar Fish Collector quest!");
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

    public class KeeperOfSecretsThalgar : MondainQuester
    {
        [Constructable]
        public KeeperOfSecretsThalgar()
            : base("The Keeper of Secrets", "Thalgar")
        {
        }

        public KeeperOfSecretsThalgar(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Thalgar's Oceanic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Thalgar's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Thalgar's Mysterious Ring" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Thalgar's Secret Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalgar's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PeculiarFishCollectorQuest)
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
