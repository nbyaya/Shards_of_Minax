using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceFeelingCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Serenity"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Lyra, the Essence Collector. I require your aid in gathering 50 EssenceFeelings. " +
                       "These essences are crucial for my research on tranquility and balance. Your efforts will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and a unique Essence Collector's Robe that will signify your contribution to the cause.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, come back to me with the essences."; } }

        public override object Uncomplete { get { return "I still need 50 EssenceFeelings. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 EssenceFeelings I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May peace and serenity be with you!"; } }

        public EssenceFeelingCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceFeeling), "EssenceFeeling", 50, 0x571C)); // Assuming EssenceFeeling item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MinersSturdyBoots), 1, "Essence Collector's Robe")); // Custom item or Robe with unique appearance
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Serenity quest!");
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

    public class EssenceCollectorLyra : MondainQuester
    {
        [Constructable]
        public EssenceCollectorLyra()
            : base("The Essence Collector", "Lyra")
        {
        }

        public EssenceCollectorLyra(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Lyra's Essence Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Lyra's Bloom" }); // Custom item for unique look
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Lyra's Essence Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceFeelingCollectorQuest)
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
