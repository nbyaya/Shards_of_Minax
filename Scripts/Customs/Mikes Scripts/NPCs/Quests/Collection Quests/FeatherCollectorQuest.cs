using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FeatherCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Feather Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Eldara, the Feather Sage. I require your assistance to collect 500 Feathers. " +
                       "These feathers are vital for my magical experiments and potion brewing. As a token of my appreciation, you will receive " +
                       "gold, a rare Maxxia Scroll, and a unique Feather Sage's Leggings to commemorate your achievement.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Feathers."; } }

        public override object Uncomplete { get { return "I still need 50 Feathers. Please gather them and bring them to me."; } }

        public override object Complete { get { return "Excellent! You have gathered the 50 Feathers I requested. Your help is greatly appreciated. " +
                       "Please accept these rewards as a mark of my gratitude. May your journey be blessed!"; } }

        public FeatherCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Feather), "Feather", 500, 0x1BD1)); // Assuming Feather item ID is 0xF7F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperingWindLeggings), 1, "Feather Sage's Leggings")); // Assuming Feather Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Feather Collector quest!");
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

    public class FeatherSageEldara : MondainQuester
    {
        [Constructable]
        public FeatherSageEldara()
            : base("The Feather Sage", "Eldara")
        {
        }

        public FeatherSageEldara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Eldara's Feather Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Eldara's Enchanted Feather Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Eldara's Magical Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Eldara's Mystical Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldara's Feather Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FeatherCollectorQuest)
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
