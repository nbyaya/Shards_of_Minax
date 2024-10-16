using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GarlicCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Garlic Gathering Task"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Mortimer, the Garlic Enthusiast. I need your help to collect 500 Garlic. " +
                       "Garlic is essential for my alchemical experiments and culinary creations. In return for your efforts, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a special Garlic Enthusiast's Attire.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Garlic."; } }

        public override object Uncomplete { get { return "I still require 500 Garlic. Please bring them to me to aid in my culinary endeavors!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 500 Garlic I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be fruitful!"; } }

        public GarlicCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(Garlic), "Garlic", 500, 0xF84)); // Assuming Garlic item ID is 0xF6F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeastmastersChest), 1, "Garlic Enthusiast's Attire")); // Assuming Garlic Enthusiast's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Garlic Gathering Task!");
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

    public class GarlicEnthusiastMortimer : MondainQuester
    {
        [Constructable]
        public GarlicEnthusiastMortimer()
            : base("The Garlic Enthusiast", "Mortimer")
        {
        }

        public GarlicEnthusiastMortimer(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Mortimer's Alchemist Apron" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new TricorneHat { Hue = Utility.Random(1, 3000), Name = "Mortimer's Garlic Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new BodySash { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Mortimer's Traveling Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GarlicCollectorQuest2)
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

    public class GarlicEnthusiastsAttire : Item
    {
        [Constructable]
        public GarlicEnthusiastsAttire() : base(0x1F00) // Assuming a placeholder item ID
        {
            Hue = Utility.Random(1, 3000);
            Name = "Garlic Enthusiast's Attire";
        }

        public GarlicEnthusiastsAttire(Serial serial) : base(serial)
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
