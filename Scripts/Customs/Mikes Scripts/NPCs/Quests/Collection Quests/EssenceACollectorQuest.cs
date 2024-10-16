using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceACollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Triumph"; } }

        public override object Description
        {
            get
            {
                return "Greetings, hero! I am Liora, the Essence Keeper. I require your aid in gathering 50 EssenceAchievements. " +
                       "These essences are crucial for my research into enhancing our magical defenses. Your dedication to this task " +
                       "will be rewarded with gold, a rare Maxxia Scroll, and a distinctive Essence Keeper's Garb that will signify " +
                       "your contribution to our cause.";
            }
        }

        public override object Refuse { get { return "I see. Should you change your mind, come back to me with the essences."; } }

        public override object Uncomplete { get { return "I still need 50 EssenceAchievements. Please bring them to me so we can proceed!"; } }

        public override object Complete { get { return "Fantastic! You've brought the 50 EssenceAchievements I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. Continue to shine in your endeavors!"; } }

        public EssenceACollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceAchievement), "Essence Achievement", 50, 0x571C)); // Assuming EssenceAchievement item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TransmutationThighBoots), 1, "Essence Keeper's Garb")); // Assuming Essence Keeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Triumph quest!");
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

    public class EssenceKeeperLiora : MondainQuester
    {
        [Constructable]
        public EssenceKeeperLiora()
            : base("The Essence Keeper", "Liora")
        {
        }

        public EssenceKeeperLiora(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Liora's Essence Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Liora's Feathered Hat" });
            AddItem(new BodySash { Hue = Utility.Random(1, 3000) });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Liora's Enchanted Staff" }); // Assuming Enchanted Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Liora's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceACollectorQuest)
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
