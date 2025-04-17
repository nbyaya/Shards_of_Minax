using System;

namespace Server.Items
{
    public class HeartseekerBow : CompositeBow
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public HeartseekerBow()
        {
            LootType = LootType.Blessed;
            Attributes.AttackChance = 5;
            Attributes.WeaponSpeed = 10;
            Attributes.WeaponDamage = 25;
            WeaponAttributes.LowerStatReq = 70;
        }

        public HeartseekerBow(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1078210;
            }
        }// HeartseekerBow
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}