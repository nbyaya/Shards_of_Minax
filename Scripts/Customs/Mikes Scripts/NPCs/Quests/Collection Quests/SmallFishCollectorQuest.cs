using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SmallFishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Aquatic Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble traveler! I am Artemis, the Aquatic Sage of the deep waters. Our oceans face a dire crisis, and " +
                       "I need your help to collect 50 SmallFish. These tiny creatures are vital for my research to restore balance to the seas. " +
                       "In return for your invaluable assistance, I shall reward you with gold, a rare Maxxia Scroll, and a one-of-a-kind Aquatic Sage's Vest, " +
                       "imbued with the essence of the deep.";
            }
        }

        public override object Refuse { get { return "Very well. Should you decide to aid the oceans later, return to me with the SmallFish."; } }

        public override object Uncomplete { get { return "I am still in need of 50 SmallFish. Please bring them to me so I can continue my important work!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 SmallFish I required. Your contribution is deeply appreciated. " +
                       "Accept these rewards as a token of my gratitude. May the depths of the ocean always guide you!"; } }

        public SmallFishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SmallFish), "SmallFish", 50, 0x0DD6)); // Assuming SmallFish item ID is 0xF6A
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FishermansVest), 1, "Aquatic Sage's Vest")); // Assuming Aquatic Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Aquatic Quest!");
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

    public class AquaticSagesRobe : BaseClothing
    {
        [Constructable]
        public AquaticSagesRobe() : base(0x1F0)
        {
            Hue = Utility.Random(1, 3000); // Unique hue for the robe
            Name = "Aquatic Sage's Robe";
        }

        public AquaticSagesRobe(Serial serial) : base(serial)
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

    public class AquaticSageArtemis : MondainQuester
    {
        [Constructable]
        public AquaticSageArtemis()
            : base("The Aquatic Sage", "Artemis")
        {
        }

        public AquaticSageArtemis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Artemis's Aquatic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Artemis's Sea Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Artemis's Enchanted Bracelet" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Artemis's Sea Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Artemis's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SmallFishCollectorQuest)
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
