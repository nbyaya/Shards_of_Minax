using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x2790, 0x27DB)] // Same visuals as LeatherNinjaBelt
    public class WrestlingBelt : BaseWrestlingBelt
    {
        [Constructable]
        public WrestlingBelt() : base(0x2790)
        {
            Weight = 1.0;
            Name = "Wrestler's Belt";
        }

        public WrestlingBelt(Serial serial) : base(serial) { }

        public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

        public override int AosStrengthReq => 0;
        public override int AosMinDamage => 5;
        public override int AosMaxDamage => 10;
        public override int AosSpeed => 50;

        public override float MlSpeed => 2.50f;

        public override int OldStrengthReq => 0;
        public override int OldMinDamage => 1;
        public override int OldMaxDamage => 6;
        public override int OldSpeed => 50;

        public override int InitMinHits => 30;
        public override int InitMaxHits => 40;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Wrestling");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
