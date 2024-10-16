using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class TurquoiseCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Turquoise Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, intrepid traveler! I am Zoltar, the Keeper of the Turquoise. My ancient lore speaks of a rare and " +
                       "mystical turquoise that holds the key to unlocking the hidden realms of our world. I need your help to collect " +
                       "50 Turquoise. These precious gems are essential for a grand ritual that will protect our lands from impending doom. " +
                       "In gratitude for your aid, I will reward you with gold, a rare Maxxia Scroll, and a unique Turquoise Keeper's Garb, " +
                       "imbued with the essence of the turquoise itself.";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, return to me with the Turquoise."; } }

        public override object Uncomplete { get { return "I still require 50 Turquoise. Please gather them to aid in our grand ritual!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Turquoise I needed. Your assistance is crucial to our protection. " +
                       "Accept these rewards as a token of my appreciation. May you be blessed with courage on your journeys!"; } }

        public TurquoiseCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Turquoise), "Turquoise", 50, 0x3193)); // Assuming Turquoise item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChestplateOfEternalFlame), 1, "Turquoise Keeper's Garb")); // Assuming Turquoise Keeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Turquoise Collector quest!");
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

    public class TurquoiseKeeperZoltar : MondainQuester
    {
        [Constructable]
        public TurquoiseKeeperZoltar()
            : base("The Turquoise Keeper", "Zoltar")
        {
        }

        public TurquoiseKeeperZoltar(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Zoltar's Turquoise Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Zoltar's Enchanted Bracelet" });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Zoltar's Ritual Cap" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zoltar's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(TurquoiseCollectorQuest)
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
