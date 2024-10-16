using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialLongbow : Bow
{
    [Constructable]
    public CelestialLongbow()
    {
        Name = "Celestial Longbow";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.LowerRegCost = 20;
        Attributes.NightSight = 1;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.HitLeechMana = 30;
        SkillBonuses.SetValues(0, SkillName.Archery, 35.0);
        SkillBonuses.SetValues(1, SkillName.Chivalry, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialLongbow(Serial serial) : base(serial)
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
