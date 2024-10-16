using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BootsOfBalladry : PlateGorget
{
    [Constructable]
    public BootsOfBalladry()
    {
        Name = "Boots of Balladry";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 80);
        AbsorptionAttributes.CastingFocus = 15;
        ArmorAttributes.SelfRepair = 10;
        Attributes.RegenHits = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);
        ColdBonus = 20;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BootsOfBalladry(Serial serial) : base(serial)
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
