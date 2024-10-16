using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReflectionShield : VikingSword
{
    [Constructable]
    public ReflectionShield()
    {
        Name = "Reflection Shield";
        Hue = Utility.Random(150, 2900);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.ReflectPhysical = 10;
        Attributes.BonusHits = 10;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.ResistPhysicalBonus = 10;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReflectionShield(Serial serial) : base(serial)
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
