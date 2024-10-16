using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HollowPrismCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Hollow Prism Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Aeloria, the Mystical Seer. My magical scrying has revealed a dire need for " +
                       "Hollow Prisms. These prisms are vital for harnessing the arcane energies that will protect our realm from " +
                       "a looming darkness. Please, gather 50 of these precious artifacts and bring them to me. In return, I will " +
                       "reward you with gold, a rare Maxxia Scroll, and the enigmatic Seer's Garb, imbued with magical hues.";
            }
        }

        public override object Refuse { get { return "I see. Should you change your mind, return to me with the Hollow Prisms."; } }

        public override object Uncomplete { get { return "I still need 50 Hollow Prisms. Your help is essential for safeguarding our world!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Hollow Prisms I needed. Your efforts have not gone unnoticed. " +
                       "Please accept these rewards as a token of my gratitude. May the stars guide you on your journey!"; } }

        public HollowPrismCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(HollowPrism), "Hollow Prisms", 50, 0x2F5D)); // Assuming HollowPrism item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MaulersHelmOfMastery), 1, "Seer's Garb")); // Assuming Seer's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Hollow Prism Collector quest!");
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

    public class SeerAeloria : MondainQuester
    {
        [Constructable]
        public SeerAeloria()
            : base("The Mystical Seer", "Aeloria")
        {
        }

        public SeerAeloria(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Aeloria's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Aeloria's Enchanted Hat" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Aeloria's Mystical Pants" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Aeloria's Arcane Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aeloria's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HollowPrismCollectorQuest)
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
