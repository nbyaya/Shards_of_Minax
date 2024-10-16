using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SorrowsGrasp : PlateGloves
{
    [Constructable]
    public SorrowsGrasp()
    {
        Name = "Sorrow's Grasp";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(25, 65);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.SelfRepair = -3;
        Attributes.IncreasedKarmaLoss = 10;
        Attributes.Luck = -30;
        SkillBonuses.SetValues(0, SkillName.Parry, -10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SorrowsGrasp(Serial serial) : base(serial)
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
