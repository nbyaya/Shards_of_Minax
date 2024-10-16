using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class SwampDragonFeeder : Barrel
    {
        private static List<SwampDragonFeeder> Feeders = new List<SwampDragonFeeder>();

        [Constructable]
        public SwampDragonFeeder()
        {
            Name = "Dragon Feeder";
			ItemID = 0x0E83;
            Feeders.Add(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Place Cantaloupes inside to raise ScaledSwampDragons");
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            foreach (SwampDragonFeeder feeder in Feeders)
            {
                if (feeder != null && !feeder.Deleted)
                {
                    feeder.CheckAndSpawnDragon();
                }
            }
        }

        private void CheckAndSpawnDragon()
        {
            Item cantaloupe = FindItemByType(typeof(Cantaloupe));

            if (cantaloupe != null && cantaloupe.Amount > 0)
            {
                cantaloupe.Consume(1); // Consume one cantaloupe
                Point3D dragonLocation = GetRandomAdjacentEmptyLocation();
                if (Map != null && dragonLocation != Point3D.Zero)
                {
                    ScaledSwampDragon dragon = new ScaledSwampDragon();
                    dragon.MoveToWorld(dragonLocation, Map);
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

        public SwampDragonFeeder(Serial serial) : base(serial)
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
