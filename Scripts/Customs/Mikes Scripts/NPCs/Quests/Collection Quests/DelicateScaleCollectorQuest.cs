using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DelicateScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scale Collector's Task"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Drystan, the Scale Collector. I seek your help to gather 50 Delicate Scales, " +
                       "as they are essential for my research and potion crafting. Your assistance would be greatly valued. " +
                       "In return, I shall reward you with gold, a rare Maxxia Scroll, and a unique Scale Armor set.";
            }
        }

        public override object Refuse { get { return "If you change your mind, come back with the scales and I'll be glad to help you.";} }

        public override object Uncomplete { get { return "I am still in need of 50 Delicate Scales. Please bring them to me as soon as possible."; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Delicate Scales. Your help has been invaluable. " +
                       "Here are your rewards. May they aid you well in your travels!"; } }

        public DelicateScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DelicateScales), "Delicate Scale", 50, 0x573A)); // Assuming Delicate Scale item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ShanachiesStorytellingShoes), 1, "Unique Scale Armor Set")); // Assuming Scale Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scale Collector's Task quest!");
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

    public class DrystanTheScaleCollector : MondainQuester
    {
        [Constructable]
        public DrystanTheScaleCollector()
            : base("Drystan the Scale Collector", "Drystan")
        {
        }

        public DrystanTheScaleCollector(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Drystan's Scale Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Drystan's Scale Hat" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Drystan's Enchanted Staff" }); // Assuming Enchanted Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Drystan's Mysterious Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DelicateScaleCollectorQuest)
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
