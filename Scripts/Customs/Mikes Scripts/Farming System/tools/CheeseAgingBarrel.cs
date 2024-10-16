using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Items
{
    public class CheeseAgingBarrel : Barrel
    {
        private static List<CheeseAgingBarrel> Barrels = new List<CheeseAgingBarrel>();

        [Constructable]
        public CheeseAgingBarrel() 
        {
            Name = "Cheese Aging Barrel";
			ItemID = 0x0FAE;

            Barrels.Add(this);
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            foreach (CheeseAgingBarrel cheesebarrel in Barrels)
            {
                if (cheesebarrel != null && !cheesebarrel.Deleted)
                {
                    Item cheeseWheel = cheesebarrel.FindItemByType(typeof(CheeseWheel));

                    if (cheeseWheel != null && cheeseWheel.Amount > 0)
                    {
                        cheeseWheel.Consume(1); // Reduce the number of CheeseWheels by one
                        cheesebarrel.DropItem(new RandomFancyCheese()); // Add a RandomFancyCheese to the barrel
                    }
                }
            }
        }

        public CheeseAgingBarrel(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Barrels.Add(this);
        }
    }
}
