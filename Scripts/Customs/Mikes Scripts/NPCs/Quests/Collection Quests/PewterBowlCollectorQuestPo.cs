using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PewterBowlCollectorQuestPo : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Pewter Bowl Collector"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave adventurer! I am Balthazar, the eccentric collector of rare and curious items. " +
                       "My latest obsession is the humble Pewter Bowl Of Potatoes. These bowls hold a strange enchantment that " +
                       "fascinates me. I need you to gather 50 of these peculiar items. In exchange, I shall reward you with a handsome sum of gold, " +
                       "a rare Maxxia Scroll, and a uniquely adorned Collector's Gauntlets that will mark you as a true seeker of rare artifacts.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Pewter Bowls Of Potatoes."; } }

        public override object Uncomplete { get { return "I still require 50 Pewter Bowls Of Potatoes. Please bring them to me so that I may complete my collection!"; } }

        public override object Complete { get { return "Splendid work! You have brought me the 50 Pewter Bowls Of Potatoes I sought. Your dedication is commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your quests be as fruitful as this one!"; } }

        public PewterBowlCollectorQuestPo() : base()
        {
            AddObjective(new ObtainObjective(typeof(PewterBowlOfPotatos), "Pewter Bowls Of Potatoes", 50, 0x1602)); // Assuming PewterBowlOfPotatoes item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(KnightsGauntlets), 1, "Collector's Gauntlets")); // Assuming Collector's Vest is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Pewter Bowl Collector quest!");
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

    public class CollectorBalthazar : MondainQuester
    {
        [Constructable]
        public CollectorBalthazar()
            : base("The Collector", "Balthazar")
        {
        }

        public CollectorBalthazar(Serial serial)
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
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Balthazar's Collector's Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new TricorneHat { Hue = Utility.Random(1, 3000), Name = "Balthazar's Unique Hat" });
            AddItem(new GoldNecklace { Hue = Utility.Random(1, 3000), Name = "Balthazar's Enchanted Necklace" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Balthazar's Collector's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PewterBowlCollectorQuestPo)
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
