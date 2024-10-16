using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class HiryuFeeder : Barrel
    {
        private static List<HiryuFeeder> Feeders = new List<HiryuFeeder>();

        [Constructable]
        public HiryuFeeder()
        {
            Name = "Hiryu Feeder";
            ItemID = 0x0E83;
            Feeders.Add(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Place Cantaloupes inside to raise Hiryu");
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            foreach (HiryuFeeder feeder in Feeders)
            {
                if (feeder != null && !feeder.Deleted)
                {
                    feeder.CheckAndSpawnHiryu();
                }
            }
        }

        private void CheckAndSpawnHiryu()
        {
            Item cantaloupe = FindItemByType(typeof(Cantaloupe));

            if (cantaloupe != null && cantaloupe.Amount > 0)
            {
                cantaloupe.Consume(1); // Consume one cantaloupe
                Point3D hiryuLocation = GetRandomAdjacentEmptyLocation();
                if (Map != null && hiryuLocation != Point3D.Zero)
                {
                    Hiryu hiryu = new Hiryu();
                    hiryu.MoveToWorld(hiryuLocation, Map);
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

        public HiryuFeeder(Serial serial) : base(serial)
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
