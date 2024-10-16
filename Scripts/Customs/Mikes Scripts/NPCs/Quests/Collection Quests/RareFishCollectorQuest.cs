using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RareFishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Rare Fish Collector"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave traveler! I am Aeloria, the Keeper of Tides. " +
                       "I am in dire need of 50 TrulyRareFish for my oceanic research. These fish are extremely elusive and precious, " +
                       "harboring secrets of the deep. In return for your heroic efforts, I shall bestow upon you a handsome reward: " +
                       "a trove of gold, a rare Maxxia Scroll, and a fantastical outfit that will mark you as a true collector of the rarest of treasures.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, bring me the TrulyRareFish."; } }

        public override object Uncomplete { get { return "I still need 50 TrulyRareFish. Bring them to me to aid my research of the deep oceans!"; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 TrulyRareFish I required. Your contribution is immensely valuable. " +
                       "Please accept these rewards as a token of my gratitude. May your future be filled with adventurous finds and wondrous discoveries!"; } }

        public RareFishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(TrulyRareFish), "TrulyRareFish", 50, 0xDD6)); // Assuming TrulyRareFish item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SamuraisDestinyHauberk), 1, "Mystic Collector's Outfit")); // Assuming MysticOutfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Rare Fish Collection quest!");
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

    public class KeeperOfTidesAeloria : MondainQuester
    {
        [Constructable]
        public KeeperOfTidesAeloria()
            : base("The Keeper of Tides", "Aeloria")
        {
        }

        public KeeperOfTidesAeloria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 50);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Aeloria's Seaweave Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new TricorneHat { Hue = Utility.Random(1, 3000), Name = "Aeloria's Tide Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Aeloria's Oceanic Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aeloria's Aquatic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RareFishCollectorQuest)
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
