using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MedusaScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Medusa Scale Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave hero! I am Thalric, the Guardian of the Gorgon Cavern. The Medusa Dark Scales are vital to my research " +
                       "on the ancient serpent magic. These scales are rare and dangerous to collect, but they hold secrets that could reshape " +
                       "the world. I need you to gather 50 Medusa Dark Scales for me. In exchange for your bravery, I will reward you with gold, " +
                       "a rare Maxxia Scroll, and a unique Medusa Guardian's Garb that will mark your valor.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Medusa Dark Scales."; } }

        public override object Uncomplete { get { return "I still require 50 Medusa Dark Scales. Bring them to me so that my research may proceed!"; } }

        public override object Complete { get { return "Fantastic work! You've collected the 50 Medusa Dark Scales I needed. Your courage is commendable. " +
                       "Please accept these rewards as a symbol of my gratitude. May your adventures be ever thrilling!"; } }

        public MedusaScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MedusaDarkScales), "Medusa Dark Scales", 50, 0x1F5)); // Assuming Medusa Dark Scale item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MendersDivineRobe), 1, "Medusa Guardian's Garb")); // Assuming Medusa Guardian's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Medusa Scale Collector quest!");
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

    public class MedusaGuardianThalric : MondainQuester
    {
        [Constructable]
        public MedusaGuardianThalric()
            : base("The Guardian of the Gorgon Cavern", "Thalric")
        {
        }

        public MedusaGuardianThalric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new ChainChest { Hue = Utility.Random(1, 3000), Name = "Thalric's Gorgon Armor" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new Helmet { Hue = Utility.Random(1, 3000), Name = "Thalric's Gorgon Helm" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Thalric's Guardian Ring" });
            AddItem(new Shield { Hue = Utility.Random(1, 3000), Name = "Thalric's Gorgon Shield" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalric's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MedusaScaleCollectorQuest)
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
