using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DaemonClawCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Daemon Hunter's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant warrior! I am Korrath, the Daemon Hunter. I need your help to collect 50 Daemon Claws, " +
                       "which are vital for my preparations against the daemonic forces. Your bravery in gathering these claws will be " +
                       "rewarded with gold, a rare Maxxia Scroll, and a unique Daemon Hunter's Garb that will mark you as a true champion.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the claws."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Daemon Claws. Please bring them to me so that I may proceed with my plans!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Daemon Claws I required. Your assistance has been invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your courage continue to shine brightly!"; } }

        public DaemonClawCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DaemonClaw), "Daemon Claw", 50, 0x5721)); // Assuming Daemon Claw item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TidecallersSandals), 1, "Daemon Hunter's Garb")); // Assuming Daemon Hunter's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Daemon Hunter's Request quest!");
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

    public class DaemonHunterKorrath : MondainQuester
    {
        [Constructable]
        public DaemonHunterKorrath()
            : base("The Daemon Hunter", "Korrath")
        {
        }

        public DaemonHunterKorrath(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Korrath's Daemon Armor" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Korrath's Daemon Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new Longsword { Hue = Utility.Random(1, 3000), Name = "Korrath's Daemon Slayer" }); // Assuming Daemon Slayer is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Korrath's Adventurer's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DaemonClawCollectorQuest)
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
