using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class BullFrogFeeder : Barrel
    {
        private static List<BullFrogFeeder> Feeders = new List<BullFrogFeeder>();

        [Constructable]
        public BullFrogFeeder()
        {
            Name = "BullFrog Feeder";
            ItemID = 0x0E83;
            Feeders.Add(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Place Bananas inside to raise BullFrogs");
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            foreach (BullFrogFeeder feeder in Feeders)
            {
                if (feeder != null && !feeder.Deleted)
                {
                    feeder.CheckAndSpawnBullFrog();
                }
            }
        }

        private void CheckAndSpawnBullFrog()
        {
            Item bananas = FindItemByType(typeof(Bananas));

            if (bananas != null && bananas.Amount > 0)
            {
                bananas.Consume(1); // Consume one banana
                Point3D bullFrogLocation = GetRandomAdjacentEmptyLocation();
                if (Map != null && bullFrogLocation != Point3D.Zero)
                {
                    BullFrog bullFrog = new BullFrog();
                    bullFrog.MoveToWorld(bullFrogLocation, Map);
                }
            }
        }

        private Point3D GetRandomAdjacentEmptyLocation()
        {
            List<Point3D> emptyLocations = new List<Point3D>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue; // Skip the center spot
                    Point3D testPoint = new Point3D(X + x, Y + y, Z);
                    if (Map != null && Map.CanSpawnMobile(testPoint))
                        emptyLocations.Add(testPoint);
                }
            }

            if (emptyLocations.Count > 0)
                return emptyLocations[Utility.Random(emptyLocations.Count)];

            return Point3D.Zero; // No valid location found
        }

        public BullFrogFeeder(Serial serial) : base(serial)
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

            Feeders.Add(this);
        }
    }
}
