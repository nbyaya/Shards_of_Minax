using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class VoidCoreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Void Core Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Arion, the Keeper of the Void. For centuries, I have guarded the secrets of the Void " +
                       "and now I seek your assistance in collecting 50 VoidCores. These powerful artifacts are essential for maintaining the balance " +
                       "between dimensions. As a token of my gratitude, I will reward you with gold, a rare Maxxia Scroll, and a unique Voidkeeper's Attire.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the VoidCores."; } }

        public override object Uncomplete { get { return "I still need 50 VoidCores. Return to me once you have collected them to aid in my quest!"; } }

        public override object Complete { get { return "Well done! You have collected the 50 VoidCores I requested. Your bravery and dedication are commendable. " +
                       "Accept these rewards as a sign of my appreciation. May you always walk the path of balance!"; } }

        public VoidCoreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VoidCore), "VoidCores", 50, 0x5728)); // Assuming VoidCore item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HippiesPeacefulSandals), 1, "Voidkeeper's Attire")); // Assuming Voidkeeper's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Void Core Collector quest!");
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

    public class VoidkeeperArion : MondainQuester
    {
        [Constructable]
        public VoidkeeperArion()
            : base("The Voidkeeper", "Arion")
        {
        }

        public VoidkeeperArion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Arion's Voidkeeper Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Arion's Voidkeeper Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Arion's Voidkeeper Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Arion's Dimensional Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VoidCoreCollectorQuest)
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
