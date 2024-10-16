using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BombDisposalPlate : PlateChest
{
    [Constructable]
    public BombDisposalPlate()
    {
        Name = "Bomb Disposal Plate";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(30, 80);
        AbsorptionAttributes.EaterFire = 40;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 15;
        Attributes.BonusHits = 30;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 30;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BombDisposalPlate(Serial serial) : base(serial)
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
