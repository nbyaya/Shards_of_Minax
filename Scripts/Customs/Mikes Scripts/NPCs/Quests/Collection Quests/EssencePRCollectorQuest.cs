using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssencePRCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence of Precision"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Alaric, the Keeper of Mystical Arts. I seek your aid in collecting 50 Essence of Precision, " +
                       "which are crucial for my latest magical research. Your efforts will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a distinctive garb that showcases your prowess.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Essence of Precision."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Essence of Precision. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Essence of Precision I required. Your contribution is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. Continue to pursue greatness!"; } }

        public EssencePRCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssencePrecision), "Essence of Precision", 50, 0x571C)); // Assuming EssencePrecision item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ArtisansTimberShoes), 1, "Alaric's Enchanted Garb")); // Assuming AlchemyRobe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence of Precision quest!");
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

    public class AlaricTheMage : MondainQuester
    {
        [Constructable]
        public AlaricTheMage()
            : base("The Keeper of Mystical Arts", "Alaric")
        {
        }

        public AlaricTheMage(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Alaric's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Alaric's Enchanted Cloak" });
            AddItem(new Circlet { Hue = Utility.Random(1, 3000), Name = "Alaric's Arcane Circlet" });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Alaric's Staff of Wisdom" }); // Assuming Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Alaric's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssencePRCollectorQuest)
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
