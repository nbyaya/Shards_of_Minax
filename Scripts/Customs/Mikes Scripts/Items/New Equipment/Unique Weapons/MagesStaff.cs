using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MagesStaff : BlackStaff
{
    [Constructable]
    public MagesStaff()
    {
        Name = "Mage's Staff";
        Hue = Utility.Random(100, 2900);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 35;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MagesStaff(Serial serial) : base(serial)
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
