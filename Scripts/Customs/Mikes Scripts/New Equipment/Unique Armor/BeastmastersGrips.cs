using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersGrips : LeatherGloves
{
    [Constructable]
    public BeastmastersGrips()
    {
        Name = "Beastmaster's Grips";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(18, 52);
        AbsorptionAttributes.EaterFire = 20;
        Attributes.BonusStr = 20;
        Attributes.RegenStam = 4;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersGrips(Serial serial) : base(serial)
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
