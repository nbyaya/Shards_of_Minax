using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssencePCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Persistence"; } }

        public override object Description
        {
            get
            {
                return "Greetings, intrepid adventurer! I am Lyra, the Essence Keeper. I need your aid to gather 50 EssencePersistence, " +
                       "which are crucial for my studies on the power of persistence. Your help in this endeavor will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and a unique Essence Keeper's Garb that will mark you as a true seeker of knowledge.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the EssencePersistence."; } }

        public override object Uncomplete { get { return "I still require 50 EssencePersistence. Bring them to me so I can continue my research!"; } }

        public override object Complete { get { return "Excellent! You have collected the 50 EssencePersistence I needed. Your dedication is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your quest for knowledge continue!"; } }

        public EssencePCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssencePersistence), "EssencePersistence", 50, 0x571C)); // Assuming EssencePersistence item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RapierMastersArms), 1, "Essence Keeper's Garb")); // Assuming Essence Keeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Persistence quest!");
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

    public class EssenceKeeperLyra : MondainQuester
    {
        [Constructable]
        public EssenceKeeperLyra()
            : base("The Essence Keeper", "Lyra")
        {
        }

        public EssenceKeeperLyra(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Lyra's Essence Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Lyra's Essence Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Lyra's Tome of Knowledge" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Lyra's Knowledge Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssencePCollectorQuest)
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
