using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PowersBeacon : GearLauncher
{
    [Constructable]
    public PowersBeacon()
    {
        Name = "Power's Beacon";
        Hue = Utility.Random(10, 2150);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.BonusMana = 20;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PowersBeacon(Serial serial) : base(serial)
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
