using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SousChefsPrecisionGloves : LeatherGloves
{
    [Constructable]
    public SousChefsPrecisionGloves()
    {
        Name = "Sous-chef's Precision Gloves";
        Hue = 1153;
        BaseArmorRating = Utility.RandomMinMax(18, 22);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusDex = 30;
        Attributes.BonusInt = 20;
        Attributes.ReflectPhysical = 15;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Cooking, 50.0);
        SkillBonuses.SetValues(1, SkillName.Fishing, 30.0);
        SkillBonuses.SetValues(2, SkillName.Alchemy, 30.0);
        PhysicalBonus = 10;
        FireBonus = 10;
        ColdBonus = 25;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SousChefsPrecisionGloves(Serial serial) : base(serial)
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
