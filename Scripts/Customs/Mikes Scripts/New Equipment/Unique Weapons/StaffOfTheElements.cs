using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StaffOfTheElements : BlackStaff
{
    [Constructable]
    public StaffOfTheElements()
    {
        Name = "Staff of the Elements";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 85);
        Attributes.SpellChanneling = 1;
        Attributes.BonusMana = 20;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.ElementalHealth;
        WeaponAttributes.HitFireArea = 20;
        WeaponAttributes.HitColdArea = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StaffOfTheElements(Serial serial) : base(serial)
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
