using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CaduceusStaff : QuarterStaff
{
    [Constructable]
    public CaduceusStaff()
    {
        Name = "Caduceus Staff";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.CastSpeed = 3;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitLeechMana = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CaduceusStaff(Serial serial) : base(serial)
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
