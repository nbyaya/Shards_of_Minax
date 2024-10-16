using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsArmguards : PlateArms
{
    [Constructable]
    public HammerlordsArmguards()
    {
        Name = "Hammerlord's Armguards";
        Hue = Utility.Random(350, 650);
        BaseArmorRating = Utility.RandomMinMax(35, 75);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.SelfRepair = 5;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
        PhysicalBonus = 20;
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsArmguards(Serial serial) : base(serial)
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
