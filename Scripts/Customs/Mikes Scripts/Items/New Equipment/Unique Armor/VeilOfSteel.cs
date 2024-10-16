using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VeilOfSteel : PlateHelm
{
    [Constructable]
    public VeilOfSteel()
    {
        Name = "Veil of Steel";
        Hue = Utility.Random(100, 400);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStr = 25;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VeilOfSteel(Serial serial) : base(serial)
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
