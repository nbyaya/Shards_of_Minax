using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FrenchBreadCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mysterious French Bread"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Gaston, the Eccentric Baker of Bucs Den. I am in desperate need of 50 French Breads. " +
                       "These loaves are not ordinary—they are enchanted with ancient culinary spells that can reveal the true essence of flavor. " +
                       "Help me gather these breads and I shall reward you with gold, a rare Maxxia Scroll, and a truly magnificent Baker's Ensemble, " +
                       "imbued with the spirit of culinary magic.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the French Breads."; } }

        public override object Uncomplete { get { return "I still need those 50 French Breads to complete my magical recipe. Please bring them to me!"; } }

        public override object Complete { get { return "Marvelous! You've brought me the 50 French Breads I needed. Your efforts are greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be as flavorful as the bread you have collected!"; } }

        public FrenchBreadCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FrenchBread), "French Breads", 50, 0x98C)); // Assuming French Bread item ID is 0x1039
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BootsOfTheDeepCaverns), 1, "Baker's Ensemble")); // Assuming Baker's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the French Bread Collector quest!");
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

    public class BakerGaston : MondainQuester
    {
        [Constructable]
        public BakerGaston()
            : base("The Eccentric Baker", "Gaston")
        {
        }

        public BakerGaston(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Gaston’s Culinary Vest" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Gaston’s Baker’s Cap" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gaston’s Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FrenchBreadCollectorQuest)
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
