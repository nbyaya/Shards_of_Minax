using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MandrakeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mandrake's Secret"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Grimgor, the Mandrake Sage. The Mandrake Root holds potent magical properties " +
                       "that I need to harness for an ancient ritual. Gather 500 Mandrake Roots for me, and in return, I will bestow upon you " +
                       "a reward worthy of your efforts: gold, a rare Maxxia Scroll, and an enchanted Hat of unparalleled design.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Mandrake Roots."; } }

        public override object Uncomplete { get { return "I still need 500 Mandrake Roots. Bring them to me, and I shall reward you handsomely!"; } }

        public override object Complete { get { return "Excellent work! You have collected the 500 Mandrake Roots I required. Your efforts have aided in a great magical ritual. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures continue to be prosperous!"; } }

        public MandrakeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MandrakeRoot), "Mandrake Roots", 500, 0xF86)); // Assuming Mandrake Root item ID is 0x1C4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CartographersHat), 1, "Mandrake Sage's Hat")); // Assuming Mandrake Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mandrake's Secret quest!");
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

    public class MandrakeSageGrimgor : MondainQuester
    {
        [Constructable]
        public MandrakeSageGrimgor()
            : base("The Mandrake Sage", "Grimgor")
        {
        }

        public MandrakeSageGrimgor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204A; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Grimgor's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Grimgor's Mystical Hat" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Grimgor's Ritual Cloak" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Grimgor's Ancient Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grimgor's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MandrakeCollectorQuest)
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
