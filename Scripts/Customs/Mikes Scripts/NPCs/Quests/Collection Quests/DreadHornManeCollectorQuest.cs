using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DreadHornManeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dread Horn Mane Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Thorne, the Guardian of the Forsaken Grove. Long ago, the " +
                       "ancient Dread Horns roamed these lands, their manes possessing untold magical power. These manes " +
                       "were said to grant strength and wisdom to those who sought their true potential. " +
                       "However, their essence has been lost to time. I need your help to collect 50 Dread Horn Manes to " +
                       "restore their power to the world. In return for your valor, I shall reward you with gold, a rare " +
                       "Maxxia Scroll, and a unique piece of armor infused with the essence of the Dread Horns.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the Dread Horn Manes."; } }

        public override object Uncomplete { get { return "I still require 50 Dread Horn Manes. Please bring them to me to restore their ancient power!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Dread Horn Manes I needed. Your bravery is unmatched. " +
                       "Accept these rewards as a token of my appreciation. May you continue to walk the path of greatness!"; } }

        public DreadHornManeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DreadHornMane), "Dread Horn Manes", 50, 0x318A)); // Assuming Dread Horn Mane item ID is 0x3B2
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CyberpunkNinjaTabi), 1, "Ancient Dread Horn Armor")); // Assuming Ancient Dread Horn Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dread Horn Mane Collector quest!");
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

    public class GuardianThorne : MondainQuester
    {
        [Constructable]
        public GuardianThorne()
            : base("The Guardian of the Forsaken Grove", "Thorne")
        {
        }

        public GuardianThorne(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Thorne's Dread Horn Armor" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGorget { Hue = Utility.Random(1, 3000) });
            AddItem(new VikingSword { Hue = Utility.Random(1, 3000), Name = "Thorne's Ancient Blade" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorne's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DreadHornManeCollectorQuest)
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
