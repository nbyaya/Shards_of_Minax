using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StaffOfRainsWrath : GnarledStaff
{
    [Constructable]
    public StaffOfRainsWrath()
    {
        Name = "Staff of Rain's Wrath";
        Hue = Utility.Random(100, 2300);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.BonusMana = 15;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitManaDrain = 25;
        WeaponAttributes.HitEnergyArea = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StaffOfRainsWrath(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
