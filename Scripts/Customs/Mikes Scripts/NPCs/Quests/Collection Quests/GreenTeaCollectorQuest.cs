using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GreenTeaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enchanted Tea Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Talindra, the Keeper of Ancient Brews. Long ago, a legendary tea was crafted from " +
                       "a rare blend of herbs and enchanted waters. This Green Tea holds the power to grant visions and insight beyond the mortal realm. " +
                       "I seek your help to collect 50 Green Teas. In return, I will bestow upon you a generous reward including gold, a rare Maxxia Scroll, " +
                       "and the exquisite Talindra's Enchanted Cap, which is said to be woven from the very essence of magical tea leaves.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Green Teas."; } }

        public override object Uncomplete { get { return "I still require 50 Green Teas. Bring them to me so that I may continue my mystical research!"; } }

        public override object Complete { get { return "Ah, you have done it! You’ve gathered the 50 Green Teas I needed. Your dedication and bravery are truly commendable. " +
                       "Accept these rewards as a token of my appreciation. May the essence of the tea guide you in your future adventures!"; } }

        public GreenTeaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GreenTea), "Green Teas", 50, 0x284C)); // Assuming Green Tea item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RangersCap), 1, "Talindra's Enchanted Cap")); // Assuming Talindra's Enchanted Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enchanted Tea Collection quest!");
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

    public class Talindra : MondainQuester
    {
        [Constructable]
        public Talindra()
            : base("The Keeper of Ancient Brews", "Talindra")
        {
        }

        public Talindra(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2042; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Talindra's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Talindra's Mystic Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Talindra's Enchanted Bracelet" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Talindra's Mystical Ring" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Talindra's Magical Spindle" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Talindra's Brew Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GreenTeaCollectorQuest)
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
