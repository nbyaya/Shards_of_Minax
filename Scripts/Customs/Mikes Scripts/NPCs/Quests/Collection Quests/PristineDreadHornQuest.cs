using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DreadHornCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dread Horn Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, courageous adventurer! I am Tharion, the Keeper of Shadows. I require your aid in gathering " +
                       "50 Pristine Dread Horns. These horns possess an eerie power and are essential for my arcane rituals " +
                       "to prevent the rise of dark forces. In exchange for your valuable assistance, I will reward you with gold, " +
                       "a rare Maxxia Scroll, and a mystical outfit that will set you apart as a true hero of the shadows.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Pristine Dread Horns."; } }

        public override object Uncomplete { get { return "I still require 50 Pristine Dread Horns. Your help is crucial in maintaining the balance between light and dark!"; } }

        public override object Complete { get { return "Outstanding! You have brought me the 50 Pristine Dread Horns I needed. Your bravery is commendable. " +
                       "Please accept these rewards as a token of my gratitude. May the shadows guide you on your path!"; } }

        public DreadHornCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PristineDreadHorn), "Pristine Dread Horns", 50, 0x315A)); // Assuming Pristine Dread Horn item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RangersHoodOfPrecision), 1, "Shadow Keeper's Outfit")); // Assuming Shadow Keeper's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dread Horn Collector quest!");
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

    public class ShadowKeeperTharion : MondainQuester
    {
        [Constructable]
        public ShadowKeeperTharion()
            : base("The Keeper of Shadows", "Tharion")
        {
        }

        public ShadowKeeperTharion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Tharion's Shadow Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new BoneHelm { Hue = Utility.Random(1, 3000), Name = "Tharion's Dark Helm" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Tharion's Shadow Gloves" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Tharion's Cloak of Shadows" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Tharion's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DreadHornCollectorQuest)
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
