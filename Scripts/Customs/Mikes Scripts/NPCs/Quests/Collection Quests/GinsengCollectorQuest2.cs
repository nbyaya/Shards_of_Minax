using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GinsengCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Ginseng Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Valtor, the Herbalist. I need your assistance to gather 500 Ginseng. " +
                       "These herbs are vital for my potions and remedies. In return for your aid, you will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and a special Ginseng Collector's Tunic.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Ginseng."; } }

        public override object Uncomplete { get { return "I still need 500 Ginseng. Please bring them to me so I can continue my work!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 500 Ginseng I required. Your help is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your journeys be fruitful!"; } }

        public GinsengCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(Ginseng), "Ginseng", 500, 0xF85)); // Assuming Ginseng item ID is 0x0C0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DiplomatsTunic), 1, "Ginseng Collector's Tunic")); // Custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Ginseng Collector quest!");
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

    public class GinsengCollectorValtor : MondainQuester
    {
        [Constructable]
        public GinsengCollectorValtor()
            : base("The Herbalist", "Valtor")
        {
        }

        public GinsengCollectorValtor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2049; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Valtor's Herbal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Valtor's Herbal Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Valtor's Enchanted Staff" }); // Custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Valtor's Herb Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GinsengCollectorQuest2)
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
