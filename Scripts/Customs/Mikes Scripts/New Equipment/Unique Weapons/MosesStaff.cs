using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MosesStaff : QuarterStaff
{
    [Constructable]
    public MosesStaff()
    {
        Name = "Moses' Staff";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 90);
        Attributes.SpellChanneling = 1;
        Attributes.BonusMana = 15;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitFireArea = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MosesStaff(Serial serial) : base(serial)
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
