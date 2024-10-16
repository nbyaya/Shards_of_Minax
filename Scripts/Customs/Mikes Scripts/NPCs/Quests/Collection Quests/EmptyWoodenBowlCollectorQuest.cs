using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EmptyWoodenBowlCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Collector of Mysteries"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Lord Dorian, Keeper of Secrets. My collection of enchanted artifacts " +
                       "is incomplete without the Empty Wooden Bowls that are crucial to my ritual. I need your assistance to procure " +
                       "50 of these bowls. They are said to hold the essence of ancient wisdom. In return for your valuable help, " +
                       "I shall reward you with gold, a rare Maxxia Scroll, and an exquisite Robe of the Collector. This robe is imbued " +
                       "with the essence of forgotten magic, making it both a garment of power and a testament to your dedication.";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, return to me with the Empty Wooden Bowls."; } }

        public override object Uncomplete { get { return "I still require 50 Empty Wooden Bowls. Please bring them to me for my ritual."; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 Empty Wooden Bowls I required. Your contribution is invaluable. " +
                       "Accept these rewards as a token of my appreciation. May your journeys be filled with wonder and discovery!"; } }

        public EmptyWoodenBowlCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EmptyWoodenBowl), "Empty Wooden Bowls", 50, 0x15F8)); // Assuming Empty Wooden Bowl item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FishmastersHauberk), 1, "Robe of the Collector")); // Assuming Robe of the Collector is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Collector of Mysteries quest!");
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

    public class LordDorian : MondainQuester
    {
        [Constructable]
        public LordDorian()
            : base("The Keeper of Secrets", "Lord Dorian")
        {
        }

        public LordDorian(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Robe of the Collector" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Hat of the Keeper" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Ring of Mysteries" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Bracelet of Secrets" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Collector's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EmptyWoodenBowlCollectorQuest)
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
