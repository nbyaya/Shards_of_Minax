using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class YellowScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scales of the Golden Serpent"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Seraphine, the Keeper of the Golden Serpent's Legacy. " +
                       "Long ago, a mighty serpent guarded a treasure of great power. It is said that its scales, " +
                       "which shimmer with the essence of the serpent's magic, hold incredible properties. I seek 50 YellowScales, " +
                       "for they are key to unlocking the secrets of this ancient guardian. " +
                       "In return for your help, I will grant you gold, a rare Maxxia Scroll, and the Seraphine's Enchanted Robe, " +
                       "a garment imbued with the serpent's grace.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the YellowScales."; } }

        public override object Uncomplete { get { return "I still require 50 YellowScales to uncover the secrets of the Golden Serpent. " +
                                                        "Please bring them to me when you have gathered them."; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 YellowScales I needed. The secrets of the Golden Serpent are within our grasp. " +
                       "As a token of my gratitude, please accept these rewards. May the serpent's blessings guide you on your journey!"; } }

        public YellowScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(YellowScales), "YellowScales", 50, 0x26B4)); // Assuming Yellow Scale item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NinjasKasa), 1, "Seraphine's Enchanted Kasa")); // Assuming Seraphine's Enchanted Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scales of the Golden Serpent quest!");
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

    public class Seraphine : MondainQuester
    {
        [Constructable]
        public Seraphine()
            : base("The Keeper of the Golden Serpent's Legacy", "Seraphine")
        {
        }

        public Seraphine(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Seraphine's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphine's Serpent Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Seraphine's Serpent Bracelet" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Seraphine's Cloak of Wisdom" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphine's Treasure Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(YellowScaleCollectorQuest)
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
