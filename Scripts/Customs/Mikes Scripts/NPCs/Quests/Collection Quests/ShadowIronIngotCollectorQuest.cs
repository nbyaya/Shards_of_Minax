using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ShadowIronIngotCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Shadow Iron Ingot Collector's Task"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I've been gathering materials for a special project, " +
                       "but I'm in need of more Shadow Iron Ingots. If you could bring me 50 Shadow Iron Ingots, " +
                       "I will reward you handsomely!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, you know where to find me."; } }

        public override object Uncomplete { get { return "You haven't collected enough Shadow Iron Ingots yet. Please keep going!"; } }

        public override object Complete { get { return "Excellent work! Here is your reward for bringing me the Shadow Iron Ingots."; } }

        public ShadowIronIngotCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ShadowIronIngot), "Shadow Iron Ingots", 50, 0x1BF2)); // Shadow Iron Ingot item ID
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold")); // Adjust the reward as necessary
            AddReward(new BaseReward(typeof(GreenwichMagesRobe), 1, "Smith's Robe")); // Example reward item
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Shadow Iron Ingot Collector's Task!");
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

    public class ShadowIronCollectorBob : MondainQuester
    {
        [Constructable]
        public ShadowIronCollectorBob()
            : base("The Smith", "Shadow Iron Collector Bob")
        {
        }

        public ShadowIronCollectorBob(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 44; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new HalfApron(33));
            AddItem(new Boots(23));
            AddItem(new SmithHammer { Name = "Bob's Smithing Hammer", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 33;
            backpack.Name = "Bag O' Shadow Iron";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ShadowIronIngotCollectorQuest)
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
