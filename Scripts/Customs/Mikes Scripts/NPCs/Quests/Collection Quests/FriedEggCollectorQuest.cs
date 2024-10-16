using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FriedEggCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Fried Egg Fiasco"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant hero! I am Zander, the Culinary Alchemist. My latest concoction requires 50 Fried Eggs, " +
                       "a crucial ingredient in my new recipe for the 'Grand Feast of Eternity'. These eggs are essential for creating a dish " +
                       "of unparalleled flavor that will be celebrated across the realms. Your efforts will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a truly magnificent outfit that reflects the brilliance of the feast.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind and wish to help, bring me the Fried Eggs."; } }

        public override object Uncomplete { get { return "I am still waiting for the 50 Fried Eggs. Please bring them to me so I can complete the feast preparations!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Fried Eggs I needed. Your contribution will ensure that the Grand Feast of Eternity is a success. " +
                       "Accept these rewards as a token of my gratitude and may your adventures be as thrilling as the feast will be!"; } }

        public FriedEggCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FriedEggs), "Fried Eggs", 50, 0x9B6)); // Assuming Fried Egg item ID is 0x9C2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WitchesBewitchingRobe), 1, "Zander's Grand Feast Attire")); // Assuming ChefRobe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Fried Egg Fiasco quest!");
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

    public class CulinaryAlchemistZander : MondainQuester
    {
        [Constructable]
        public CulinaryAlchemistZander()
            : base("The Culinary Alchemist", "Zander")
        {
        }

        public CulinaryAlchemistZander(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Zander's Grand Apron" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Zander's Culinary Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zander's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FriedEggCollectorQuest)
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
