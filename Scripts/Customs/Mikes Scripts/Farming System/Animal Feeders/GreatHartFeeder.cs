using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class GreatHartFeeder : Barrel
    {
        private static List<GreatHartFeeder> Feeders = new List<GreatHartFeeder>();

        [Constructable]
        public GreatHartFeeder()
        {
            Name = "Great Hart Feeder";
            ItemID = 0x0E83;
            Feeders.Add(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Place Dates inside to raise Great Hart");
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            foreach (GreatHartFeeder feeder in Feeders)
            {
                if (feeder != null && !feeder.Deleted)
                {
                    feeder.CheckAndSpawnGreatHart();
                }
            }
        }

        private void CheckAndSpawnGreatHart()
        {
            Item dates = FindItemByType(typeof(Dates));

            if (dates != null && dates.Amount > 0)
            {
                dates.Consume(1); // Consume one date
                Point3D hartLocation = GetRandomAdjacentEmptyLocation();
                if (Map != null && hartLocation != Point3D.Zero)
                {
                    GreatHart hart = new GreatHart();
                    hart.MoveToWorld(hartLocation, Map);
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

        public GreatHartFeeder(Serial serial) : base(serial)
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
