using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EmeraldCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Emerald Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Arcanis, the Emerald Sage. I require your aid to gather 50 Emeralds, which are " +
                       "essential for a grand magical ritual I am preparing. In return for your invaluable assistance, you will be " +
                       "rewarded with gold, a rare Maxxia Scroll, and a unique Emerald Sage's Robe that will mark your dedication to " +
                       "the mystical arts.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, come back and bring me the Emeralds."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Emeralds. Please bring them to me so that I may complete my ritual!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Emeralds I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be filled with mystical wonders!"; } }

        public EmeraldCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Emerald), "Emerald", 50, 0xF10)); // Assuming Emerald item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(KnightsHelmOfTheRoundTable), 1, "Emerald Sage's Helm")); // Assuming Emerald Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Emerald Request quest!");
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

    public class EmeraldSageArcanis : MondainQuester
    {
        [Constructable]
        public EmeraldSageArcanis()
            : base("The Emerald Sage", "Arcanis")
        {
        }

        public EmeraldSageArcanis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Arcanis' Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Arcanis' Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000) });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Arcanis' Mystic Staff" }); // Assuming Mystic Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Arcanis' Spell Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EmeraldCollectorQuest)
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
