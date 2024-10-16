using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SmithsPrecisionGauntlets : PlateGloves
{
    [Constructable]
    public SmithsPrecisionGauntlets()
    {
        Name = "Smith's Precision Gauntlets";
        Hue = Utility.Random(300, 400);
        BaseArmorRating = Utility.RandomMinMax(45, 65);
        ArmorAttributes.SelfRepair = 15;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.AttackChance = 15;
        Attributes.WeaponDamage = 20;
        SkillBonuses.SetValues(0, SkillName.Fletching, 30.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 30.0);
        PhysicalBonus = 15;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SmithsPrecisionGauntlets(Serial serial) : base(serial)
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
