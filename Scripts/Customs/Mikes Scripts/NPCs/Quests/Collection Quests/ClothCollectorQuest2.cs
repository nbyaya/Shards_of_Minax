using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ClothCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fabric of Fate"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Lysandra, the weaver of destiny. My loom is in desperate need of cloth to create a powerful " +
                       "tapestry that will reveal hidden truths about our world. I need 500 pieces of cloth to complete this sacred task. " +
                       "Will you assist me in gathering the cloth? In return, I will reward you with gold, a rare Maxxia Scroll, and a unique " +
                       "Weaver's Mantle that will mark you as a hero of the fabric arts.";
            }
        }

        public override object Refuse { get { return "I understand if you are not interested. If you change your mind, please return to me. The loom waits!"; } }

        public override object Uncomplete { get { return "I still require 500 pieces of cloth to finish my work. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "You have done it! You have gathered all the cloth I need. My tapestry will now reveal the secrets it holds. " +
                       "As a token of my gratitude, please accept these rewards. Thank you for your assistance!"; } }

        public ClothCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(Cloth), "Cloth", 500, 0x1766)); // Assuming Cloth item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChoirsHandwear), 1, "Weaver's Mantle")); // Assuming Weaver's Mantle is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fabric of Fate quest!");
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

    public class LysandraTheWeaver : MondainQuester
    {
        [Constructable]
        public LysandraTheWeaver()
            : base("The Weaver", "Lysandra the Weaver")
        {
        }

        public LysandraTheWeaver(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Classic hair style
            HairHue = 1152; // Hair hue (silver white)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Elegant robe
            AddItem(new Sandals(1152)); // Matching sandals
            AddItem(new ClothNinjaHood() { Name = "Lysandra's Veil", Hue = 1152 }); // Custom Veil
            AddItem(new BodySash() { Name = "Lysandra's Sash", Hue = 1152 }); // Custom Sash
            AddItem(new QuarterStaff() { Name = "Lysandra's Weaver's Staff", Hue = 1152 }); // Custom Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Loom Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClothCollectorQuest2)
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

    public class WeaversMantle : Item
    {
        [Constructable]
        public WeaversMantle() : base(0x1F00) // Unique item ID
        {
            Name = "Weaver's Mantle";
            Hue = 1152; // Custom hue
        }

        public WeaversMantle(Serial serial) : base(serial)
        {
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
