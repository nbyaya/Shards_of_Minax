using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StringOfEars : LeatherGorget
{
    [Constructable]
    public StringOfEars()
    {
        Name = "String of Ears";
        Hue = Utility.Random(150, 350);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterDamage = 10;
        ArmorAttributes.SelfRepair = 5;
        Attributes.LowerManaCost = 10;
        Attributes.WeaponDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StringOfEars(Serial serial) : base(serial)
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
