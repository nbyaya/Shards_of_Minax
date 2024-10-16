using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrownOfTheAbyss : CloseHelm
{
    [Constructable]
    public CrownOfTheAbyss()
    {
        Name = "Crown of the Abyss";
        Hue = Utility.Random(750, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 65);
        Attributes.WeaponDamage = 40;
        Attributes.ReflectPhysical = -30;
        Attributes.BonusStr = 25;
        SkillBonuses.SetValues(0, SkillName.Parry, -15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrownOfTheAbyss(Serial serial) : base(serial)
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
