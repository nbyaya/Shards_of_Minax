using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostwardensBascinet : Bascinet
{
    [Constructable]
    public FrostwardensBascinet()
    {
        Name = "Frostwarden's Bascinet";
        Hue = Utility.Random(600, 650);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
        ColdBonus = 25;
        EnergyBonus = 5;
        FireBonus = 0;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostwardensBascinet(Serial serial) : base(serial)
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
