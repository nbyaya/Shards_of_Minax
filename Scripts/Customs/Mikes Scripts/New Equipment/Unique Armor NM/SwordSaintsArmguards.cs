using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SwordSaintsArmguards : PlateArms
{
    [Constructable]
    public SwordSaintsArmguards()
    {
        Name = "Sword Saint's Armguards";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusDex = 25;
        Attributes.DefendChance = 20;
        Attributes.WeaponDamage = 50;
        SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        PhysicalBonus = 15;
        EnergyBonus = 15;
        ColdBonus = 10;
        FireBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SwordSaintsArmguards(Serial serial) : base(serial)
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
