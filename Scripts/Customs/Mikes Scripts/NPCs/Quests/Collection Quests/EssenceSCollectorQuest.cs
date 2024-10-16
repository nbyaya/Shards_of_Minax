using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceSCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elara, the Essence Collector. I require your aid to gather 50 Essence Singularities. " +
                       "These rare essences are crucial for my research into the mystical arts. In return for your efforts, I shall reward you with " +
                       "gold, a rare Maxxia Scroll, and a special Essence Collector's Garb that will set you apart from others.";
            }
        }

        public override object Refuse { get { return "I see. If you reconsider, return to me with the essence singularities."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Essence Singularities. Please bring them to me so that I can continue my work."; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Essence Singularities I requested. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be illuminated with wisdom!"; } }

        public EssenceSCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceSingularity), "Essence Singularity", 50, 0x571C)); // Assuming Essence Singularity item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HammerlordsCap), 1, "Essence Collector's Garb")); // Assuming Essence Collector's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence Collector's Request quest!");
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

    public class EssenceCollectorElora : MondainQuester
    {
        [Constructable]
        public EssenceCollectorElora()
            : base("The Essence Collector", "Elara")
        {
        }

        public EssenceCollectorElora(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Elara's Essence Shirt" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Elara's Mystical Pants" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Elara's Staff of Wisdom" }); // Assuming MysticStaff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elara's Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceSCollectorQuest)
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
