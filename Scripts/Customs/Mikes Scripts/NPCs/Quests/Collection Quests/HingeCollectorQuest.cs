using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class HingeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Hinge of Fate"; } }

        public override object Description
        {
            get
            {
                return "Greetings, courageous adventurer! I am Brondar, the Master Artificer. My latest project requires " +
                       "a rare and elusive component – 50 Hinges. These Hinges are not mere mechanical parts; they are imbued " +
                       "with ancient magics that are crucial for the artifact I am crafting. Your assistance in gathering them will " +
                       "be handsomely rewarded with gold, a rare Maxxia Scroll, and the unique Brondar's Artificer's Attire.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Hinges."; } }

        public override object Uncomplete { get { return "I still require 50 Hinges. Please gather them so I can complete my masterpiece!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Hinges I needed. Your contribution is invaluable. " +
                       "Accept these rewards as a token of my appreciation. May your journey be filled with prosperity!"; } }

        public HingeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Hinge), "Hinges", 50, 0x1055)); // Assuming Hinge item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(LockmastersChestplate), 1, "Brondar's Artificer's Attire")); // Assuming Brondar's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Hinge Collector quest!");
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

    public class MasterArtificerBrondar : MondainQuester
    {
        [Constructable]
        public MasterArtificerBrondar()
            : base("The Master Artificer", "Brondar")
        {
        }

        public MasterArtificerBrondar(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Brondar's Master Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new TallStrawHat { Hue = Utility.Random(1, 3000), Name = "Brondar's Artificer Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Brondar's Master Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Brondar's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HingeCollectorQuest)
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
