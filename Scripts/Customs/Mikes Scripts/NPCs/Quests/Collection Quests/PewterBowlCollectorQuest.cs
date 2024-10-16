using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class PewterBowlCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Pewter Bowl Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Thorne, the Pewter Bowl Connoisseur. My collection of Empty Pewter Bowls has been depleted, " +
                       "and I need your assistance to recover 50 of these treasured items. They are vital for my upcoming exhibition on the art of pewtercraft. " +
                       "In exchange for your help, I shall reward you with gold, a rare Maxxia Scroll, and a one-of-a-kind Pewter Artisan's Attire, crafted with care.";
            }
        }

        public override object Refuse { get { return "Very well, should you change your mind, return to me with the Empty Pewter Bowls."; } }

        public override object Uncomplete { get { return "I still require 50 Empty Pewter Bowls. Please gather them so that I may continue my exhibition preparations!"; } }

        public override object Complete { get { return "Marvelous! You have recovered the 50 Empty Pewter Bowls. Your assistance has been invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your future quests be as fruitful as this one!"; } }

        public PewterBowlCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EmptyPewterBowl), "Empty Pewter Bowls", 50, 0x15FD)); // Assuming Empty Pewter Bowl item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CourtisansRefinedGown), 1, "Pewter Artisan's Attire")); // Assuming Pewter Artisan's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Pewter Bowl Connoisseur quest!");
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

    public class PewterConnoisseurThorne : MondainQuester
    {
        [Constructable]
        public PewterConnoisseurThorne()
            : base("The Pewter Connoisseur", "Thorne")
        {
        }

        public PewterConnoisseurThorne(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Thorne's Pewter Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Thorne's Pewter Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorne's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PewterBowlCollectorQuest)
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
