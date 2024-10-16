using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class VanillaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Vanilla Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, bold explorer! I am Seraphine, the renowned Vanilla Connoisseur. My passion for the rare and exquisite Vanilla " +
                       "drives me to seek out this elusive ingredient for my magical concoctions. If you can bring me 50 Vanilla, I will reward you handsomely " +
                       "with gold, a rare Maxxia Scroll, and a truly extraordinary outfit that will mark you as a master of the vanilla arts.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Vanilla."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Vanilla. Your assistance in this matter is crucial!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 Vanilla I sought. Your dedication is most appreciated. Please accept these rewards as a token of my gratitude. " +
                       "May your future endeavors be as fruitful as this quest!"; } }

        public VanillaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Vanilla), "Vanilla", 50, 0xE2A)); // Assuming Vanilla item ID is 0x1E1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RockabillyRebelJacket), 1, "Vanilla Connoisseur's Jacket")); // Assuming Vanilla Connoisseur's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Vanilla Collector quest!");
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
}

public class VanillaConnoisseurSeraphine : MondainQuester
{
    [Constructable]
    public VanillaConnoisseurSeraphine()
        : base("The Vanilla Connoisseur", "Seraphine")
    {
    }

    public VanillaConnoisseurSeraphine(Serial serial)
        : base(serial)
    {
    }

    public override void InitBody()
    {
        InitStats(100, 100, 30);

        Body = 0x191; // Female Body

        Hue = Utility.Random(1, 3000); // Unique hue for the NPC
        HairItemID = 0x2041; // Unique hair style
        HairHue = Utility.Random(1, 3000); // Unique hair hue
    }

    public override void InitOutfit()
    {
        AddItem(new FemalePlateChest { Hue = Utility.Random(1, 3000), Name = "Seraphine's Vanilla Robe" });
        AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
        AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphine's Vanilla Hat" });
        AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Seraphine's Golden Ring" });
        AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
        Backpack backpack = new Backpack();
        backpack.Hue = Utility.Random(1, 3000);
        backpack.Name = "Seraphine's Vanilla Satchel";
        AddItem(backpack);
    }

    public override Type[] Quests
    {
        get
        {
            return new Type[]
            {
                typeof(VanillaCollectorQuest)
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
