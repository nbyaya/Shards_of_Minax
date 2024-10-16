using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TechPriestMantle : LeatherArms
{
    [Constructable]
    public TechPriestMantle()
    {
        Name = "Tech-Priest Mantle";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenMana = 7;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 20.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TechPriestMantle(Serial serial) : base(serial)
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
