using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RawLambLegCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Lamb Leg Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Ebonis, the renowned Gourmet of the realm. " +
                       "I am in dire need of 50 Raw Lamb Legs to prepare a feast for an esteemed banquet. " +
                       "These lamb legs are essential for a special dish I am crafting, one that promises to be " +
                       "the highlight of the evening. Bring them to me, and I will reward you handsomely with gold, " +
                       "a rare Maxxia Scroll, and a unique Gloves fit for a master chef!";
            }
        }

        public override object Refuse { get { return "I understand. If you reconsider, return with the Raw Lamb Legs."; } }

        public override object Uncomplete { get { return "I am still waiting for the 50 Raw Lamb Legs. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Raw Lamb Legs I needed. Your help will make this feast unforgettable. " +
                       "Accept these rewards as a token of my gratitude. May your travels be filled with adventure and flavor!"; } }

        public RawLambLegCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RawLambLeg), "Raw Lamb Legs", 50, 0x1609)); // Assuming Raw Lamb Leg item ID is 0x1036
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FalconersGloves), 1, "Master Chef's Gloves")); // Assuming Chef's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Lamb Leg Connoisseur quest!");
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

    public class EbonisTheGourmet : MondainQuester
    {
        [Constructable]
        public EbonisTheGourmet()
            : base("The Gourmet", "Ebonis")
        {
        }

        public EbonisTheGourmet(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Ebonis's Gourmet Hat" });
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Ebonis's Culinary Shirt" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Ebonis's Cooking Apron" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ebonis's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RawLambLegCollectorQuest)
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
