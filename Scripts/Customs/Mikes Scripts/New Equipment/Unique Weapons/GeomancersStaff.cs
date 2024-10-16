using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GeomancersStaff : GnarledStaff
{
    [Constructable]
    public GeomancersStaff()
    {
        Name = "Geomancer's Staff";
        Hue = Utility.Random(300, 600);
        MinDamage = Utility.RandomMinMax(20, 80);
        MaxDamage = Utility.RandomMinMax(80, 140);
        Attributes.SpellChanneling = 1;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.MageWeapon = 10;
        WeaponAttributes.ResistPhysicalBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GeomancersStaff(Serial serial) : base(serial)
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
