using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class QuicheCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Quiche Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Ah, welcome, brave adventurer! I am Gaston, the renowned Quiche Connoisseur. " +
                       "For years, I've sought to create the perfect quiche recipe, one that will transcend " +
                       "the ordinary and reach the realm of culinary perfection. To complete my quest, I need " +
                       "you to gather 50 delectable Quiches from the finest bakers in the land. In return for your " +
                       "valuable assistance, I shall reward you with gold, a rare Maxxia Scroll, and a special outfit " +
                       "that will mark you as a true patron of the culinary arts.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, return to me with the Quiches."; } }

        public override object Uncomplete { get { return "I still require 50 Quiches. Please bring them to me so that my culinary quest may continue!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Quiches I needed. Your contribution is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your culinary journeys be filled with delight!"; } }

        public QuicheCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Quiche), "Quiches", 50, 0x1041)); // Assuming Quiche item ID is 0xF9D
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BanditsHiddenCloak), 1, "Gaston’s Culinary Attire")); // Assuming ChefOutfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Quiche Connoisseur quest!");
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

    public class ChefOutfit : Item
    {
        [Constructable]
        public ChefOutfit() : base(0x1F05) // Assuming a base ID for the chef outfit
        {
            Hue = Utility.Random(1, 3000);
            Name = "Gaston’s Culinary Attire";
        }

        public ChefOutfit(Serial serial) : base(serial)
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

    public class GastonTheConnoisseur : MondainQuester
    {
        [Constructable]
        public GastonTheConnoisseur() : base("The Quiche Connoisseur", "Gaston")
        {
        }

        public GastonTheConnoisseur(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Gaston’s Fine Shirt" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Gaston’s Chef Hat" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Backpack { Hue = Utility.Random(1, 3000), Name = "Gaston’s Culinary Satchel" });
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(QuicheCollectorQuest)
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
