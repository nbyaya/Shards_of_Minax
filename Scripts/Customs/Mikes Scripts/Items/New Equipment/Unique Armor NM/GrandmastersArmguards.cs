using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrandmastersArmguards : PlateArms
{
    [Constructable]
    public GrandmastersArmguards()
    {
        Name = "Grandmaster's Armguards";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 20;
        Attributes.BonusStam = 25;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        PhysicalBonus = 12;
        ColdBonus = 12;
        FireBonus = 8;
        EnergyBonus = 18;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrandmastersArmguards(Serial serial) : base(serial)
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
