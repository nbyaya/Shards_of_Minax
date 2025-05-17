using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.MiniChamps;

namespace Server.Items
{
    public class GraveDiggerCryptMap : MagicMapBase
    {
        // –– Unique set of possible coordinates ––
		public override List<Point3D> PredefinedLocations
			=> _overridePredefinedLocations ?? new List<Point3D>
        {
			new Point3D(6058, 499, 2),
			new Point3D(5392, 2977, 0),
			new Point3D(4075, 2337, 0)
        };

        // –– Its own facet ––
		public override Map DestinationFacet
			=> _overrideDestinationFacet ?? Map.Map10;

        // –– Allowed mini-champ types ––
        public List<MiniChampType> AllowedChampTypes => new List<MiniChampType>()
        {
			MiniChampType.Encounter76
        };

        // –– One champ per tier point ––
        public int ChampsToSpawn => Tier;

        [Constructable]
        public GraveDiggerCryptMap()
            : base(0x14EB, "Map to Grave Digger Crypt", 250)
        {
        }

        public GraveDiggerCryptMap(Serial serial)
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
