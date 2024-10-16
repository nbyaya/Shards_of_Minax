using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class RawBirdCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Raw Bird Bounty"; } }

        public override object Description
        {
            get
            {
                return "Greetings, courageous adventurer! I am Vireon, the Tamer of the Skies. My avian friends have been hunted too aggressively, " +
                       "and their feathers are needed to restore balance. I require you to gather 50 RawBirds, which are essential for the ritual to " +
                       "rebalance our world's avian spirits. In return for your invaluable help, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and an outfit crafted from the finest feathers of the skies.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the RawBirds."; } }

        public override object Uncomplete { get { return "I still require 50 RawBirds to restore balance. Please bring them to me!"; } }

        public override object Complete { get { return "Excellent work! You have gathered the 50 RawBirds I needed. Your efforts are truly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May the skies always favor you!"; } }

        public RawBirdCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RawBird), "RawBirds", 50, 0x9B9)); // Assuming RawBird item ID is 0xF6C
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ForgeMastersBoots), 1, "Sky Tamer's Outfit")); // Assuming Sky Tamer's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Raw Bird Bounty quest!");
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

    public class SkyTamerVireon : MondainQuester
    {
        [Constructable]
        public SkyTamerVireon()
            : base("The Tamer of the Skies", "Vireon")
        {
        }

        public SkyTamerVireon(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Vireon's Feathered Hat" });
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Vireon's Sky Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Vireon's Sky Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RawBirdCollectorQuest)
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
