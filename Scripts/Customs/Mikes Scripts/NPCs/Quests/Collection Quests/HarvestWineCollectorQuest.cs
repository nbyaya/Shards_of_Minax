using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class HarvestWineCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Harvest Wine Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Sir Aldric the Vintner, keeper of the grand vineyard of Veridion. " +
                       "My esteemed vineyard has fallen on hard times, and I need your help to gather 50 bottles of Harvest Wine. " +
                       "These bottles are essential for restoring the vineyard's honor and to celebrate the upcoming Grand Harvest Festival. " +
                       "In exchange for your noble service, I shall reward you with gold, a rare Maxxia Scroll, and the esteemed Vintner's Attire, " +
                       "a unique garment worthy of your efforts.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind and wish to assist, return to me with the Harvest Wine."; } }

        public override object Uncomplete { get { return "I am still in need of 50 bottles of Harvest Wine. Please bring them to me so we can restore the vineyard's glory!"; } }

        public override object Complete { get { return "Splendid! You have successfully brought me the 50 bottles of Harvest Wine. Your contribution will not be forgotten. " +
                       "As a token of my gratitude, please accept these rewards. May your adventures be filled with success and joy!"; } }

        public HarvestWineCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(HarvestWine), "Harvest Wine", 50, 0xF5D)); // Assuming Harvest Wine item ID is 0xF5D
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(StarlightWizardsHat), 1, "Vintner's Attire")); // Assuming Vintner's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Harvest Wine Collector quest!");
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

    public class SirAldric : MondainQuester
    {
        [Constructable]
        public SirAldric()
            : base("The Vintner", "Sir Aldric")
        {
        }

        public SirAldric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Sir Aldric's Vintner Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Sir Aldric's Vintner Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Sir Aldric's Gold Bracelet" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Sir Aldric's Vintner Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Sir Aldric's Vintner Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HarvestWineCollectorQuest)
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
