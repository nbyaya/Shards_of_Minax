using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class WoodenBowlCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Carrot Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble traveler! I am Balthazar, the Carrot Connoisseur. My garden has been overrun with " +
                       "deliciously plump carrots, and I need your help to gather 50 Wooden Bowls of Carrots. These bowls will " +
                       "help me prepare a grand feast to celebrate the harvest. In gratitude, I shall reward you with gold, " +
                       "a rare Maxxia Scroll, and a unique Carrot Connoisseur's Attire that you will surely cherish.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, bring the Wooden Bowls of Carrots back to me."; } }

        public override object Uncomplete { get { return "I still need 50 Wooden Bowls of Carrots. Please bring them to me so that we can celebrate the harvest!"; } }

        public override object Complete { get { return "Fantastic! You've gathered the 50 Wooden Bowls of Carrots I needed. Your assistance has made this harvest " +
                       "a grand success. Please accept these rewards as a token of my appreciation. May your days be as bright as " +
                       "a carrot in the sun!"; } }

        public WoodenBowlCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfCarrots), "Wooden Bowls of Carrots", 50, 0x15F9)); // Assuming Wooden Bowl of Carrots item ID is 0x1E3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SneaksSilentShoes), 1, "Carrot Connoisseur's Attire")); // Assuming Carrot Connoisseur's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Carrot Collection quest!");
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

    public class CarrotConnoisseurBalthazar : MondainQuester
    {
        [Constructable]
        public CarrotConnoisseurBalthazar()
            : base("The Carrot Connoisseur", "Balthazar")
        {
        }

        public CarrotConnoisseurBalthazar(Serial serial)
            : base(serial)
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
            AddItem(new Tunic { Hue = Utility.Random(1, 3000), Name = "Balthazar's Carrot Tunic" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Balthazar's Carrot Bandana" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Balthazar's Carrot Cloak" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Balthazar's Carrot Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Balthazar's Carrot Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodenBowlCollectorQuest)
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
