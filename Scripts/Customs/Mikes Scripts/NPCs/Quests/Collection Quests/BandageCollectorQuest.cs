using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BandageCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Healing Hands"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elaria, a renowned healer and herbalist. " +
                       "My supplies of bandages have dwindled, and I am in dire need of 500 bandages. " +
                       "These bandages are essential for my patients and for treating wounds in our community. " +
                       "Will you assist me in gathering this vast quantity? In return, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a set of the finest Healing Sandals that I personally crafted.";
            }
        }

        public override object Refuse { get { return "I understand. Should you reconsider, please return to me. The healing of many depends on you."; } }

        public override object Uncomplete { get { return "I still require 500 bandages to complete my collection. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "You have truly outdone yourself! The bandages you have gathered will greatly aid our cause. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your generosity!"; } }

        public BandageCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bandage), "Bandage", 500, 0xE21)); // Assuming Bandage item ID is 0x1F7
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperersSandals), 1, "Healing Sandals")); // Assuming Healing Robes is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Healing Hands quest!");
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

    public class BandageCollectorElaria : MondainQuester
    {
        [Constructable]
        public BandageCollectorElaria()
            : base("The Healer", "Elaria the Herbalist")
        {
        }

        public BandageCollectorElaria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Herbalist's hair style
            HairHue = 1150; // Hair hue (bright brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1150)); // Robe in a unique hue
            AddItem(new Sandals(1150)); // Sandals matching the robe
            AddItem(new GnarledStaff { Name = "Elaria's Healing Staff", Hue = 1150 }); // Custom Healing Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Elaria's Healing Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BandageCollectorQuest)
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
