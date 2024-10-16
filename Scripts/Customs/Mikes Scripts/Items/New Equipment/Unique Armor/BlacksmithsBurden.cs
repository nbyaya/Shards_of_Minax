using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlacksmithsBurden : PlateChest
{
    [Constructable]
    public BlacksmithsBurden()
    {
        Name = "Blacksmith's Burden";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(50, 90);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 25;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 30.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 30;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlacksmithsBurden(Serial serial) : base(serial)
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
