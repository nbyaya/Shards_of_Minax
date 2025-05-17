// ─────────────────────────────────────────────────────────────────────────────
//  MERAKTUS MAGIC MAP  ––  just its unique data + champ‐spawn override
// ────────────────────────────────────────────────────────────────────────────

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.MiniChamps;

namespace Server.Items
{
    public class MeraktusTheTormentedMap : MagicMapBase
    {
        // –– Unique set of possible coordinates ––
		public override List<Point3D> PredefinedLocations
			=> _overridePredefinedLocations ?? new List<Point3D>
        {
			new Point3D(5270, 3713, 0),
			new Point3D(5628, 3686, 0),
			new Point3D(5962, 3745, 0)	
        };

        // –– Its own facet ––
		public override Map DestinationFacet
			=> _overrideDestinationFacet ?? Map.Map10;

        // –– Allowed mini-champ types ––
        public List<MiniChampType> AllowedChampTypes => new List<MiniChampType>()
        {
			MiniChampType.Encounter1,

        };

        // –– One champ per tier point ––
        public int ChampsToSpawn => Tier;

        [Constructable]
        public MeraktusTheTormentedMap()
            : base(0x14EB, "Map to Meraktus the Tormented", 1490)
        {
        }

        public MeraktusTheTormentedMap(Serial serial)
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

            // 2) (Optional) sprinkle in cupcakes!
            if (Utility.RandomDouble() < 0.99)
            {
                for (int i = 0; i < 80; i++)
                {
                    var cake = new Cake();  // your custom item
                    var cakePt = new Point3D(
                        centre.X + Utility.RandomMinMax(-2, 2),
                        centre.Y + Utility.RandomMinMax(-2, 2),
                        centre.Z);
                    cake.MoveToWorld(cakePt, map);
                    content.SpawnedEntities.Add(cake);
                }
            }

            // 3) If you also want generic monsters/chests/misc, uncomment:
            // base.SpawnChallenges(centre, map, content, owner);
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