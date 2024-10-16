using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class ClockmakerFelix : MondainQuester
    {
        [Constructable]
        public ClockmakerFelix()
            : base("Felix", "the Master Clockmaker")
        {
        }

        public ClockmakerFelix(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 50);

            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();
			AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Felix's Fine Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Felix's Tailored Pants" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000), Name = "Polished Boots" });
            AddItem(new BodySash { Hue = Utility.Random(1, 3000), Name = "Clockmaker's Tool Belt" });

            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Felix's Satchel";
            AddItem(backpack);

            // Customize appearance as needed
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ExoticGearCollectorQuest)
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
