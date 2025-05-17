using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.MiniChamps;

namespace Server.Items
{
    public class EpeeSpecialistArenaMap : MagicMapBase
    {
        // –– Unique set of possible coordinates ––
		public override List<Point3D> PredefinedLocations
			=> _overridePredefinedLocations ?? new List<Point3D>
        {
			new Point3D(4138, 1241, 10),
			new Point3D(5284, 3240, 0),
			new Point3D(1263, 2545, 0)
        };

        // –– Its own facet ––
		public override Map DestinationFacet
			=> _overrideDestinationFacet ?? Map.Map10;

        // –– Allowed mini-champ types ––
        public List<MiniChampType> AllowedChampTypes => new List<MiniChampType>()
        {
			MiniChampType.Encounter53
        };

        // –– One champ per tier point ––
        public int ChampsToSpawn => Tier;

        [Constructable]
        public EpeeSpecialistArenaMap()
            : base(0x14EB, "Map to Epee Specialist Arena", 250)
        {
        }

        public EpeeSpecialistArenaMap(Serial serial)
            : base(serial)
        {
        }

        /// <summary>
        /// Spawn the mini-champs in a loose circle, plus any extra goodies.
        /// </summary>
        protected override void SpawnChallenges(Point3D centre, Map map, SpawnedContent content, Mobile owner)
        {
            // 1) Mini-champs
            for (int i = 0; i < ChampsToSpawn; i++)
            {
                var type = AllowedChampTypes[Utility.Random(AllowedChampTypes.Count)];
                var pt = new Point3D(
                    centre.X + Utility.RandomMinMax(-10, 10),
                    centre.Y + Utility.RandomMinMax(-10, 10),
                    centre.Z);

                var ctrl = MiniChamp.CreateMiniChampOnFacet(type, SpawnRadius, pt, map);
                content.SpawnedEntities.Add(ctrl);
            }

            base.SpawnChallenges(centre, map, content, owner);
        }

        // No extra serialization needed: base handles all fields.
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version 0: no extra fields
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			// (nothing to do here for v0)
		}
		
    }
}
