using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceBalanceCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence Balance Dilemma"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elara the Enigmatic. I require your aid in collecting 50 EssenceBalances, " +
                       "which are crucial for my magical experiments. Your dedication in gathering these items will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and a unique Enigmatic Robe that will mark you as a true seeker of the arcane.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, come back with the EssenceBalances."; } }

        public override object Uncomplete { get { return "I am still in need of 50 EssenceBalances. Please gather them so we can proceed with the ritual!"; } }

        public override object Complete { get { return "Excellent work! You have brought me the 50 EssenceBalances I needed. As a token of my gratitude, " +
                       "accept these rewards. May your path be illuminated by the arcane!"; } }

        public EssenceBalanceCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceBalance), "EssenceBalance", 50, 0x571C)); // Assuming EssenceBalance item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GrandmastersParryingChest), 1, "Enigmatic Robe")); // Assuming Enigmatic Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence Balance Dilemma quest!");
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

    public class ElaraTheEnigmatic : MondainQuester
    {
        [Constructable]
        public ElaraTheEnigmatic()
            : base("The Enigmatic Sorceress", "Elara")
        {
        }

        public ElaraTheEnigmatic(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elara's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Elara's Enchanted Staff" }); // Assuming Enchanted Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elara's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceBalanceCollectorQuest)
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
