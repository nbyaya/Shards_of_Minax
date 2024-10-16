using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfTheWild : PlateGloves
{
    [Constructable]
    public GauntletsOfTheWild()
    {
        Name = "Gauntlets of the Wild";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.SelfRepair = 10;
        Attributes.RegenMana = 10;
        SkillBonuses.SetValues(0, SkillName.Tracking, 20.0);
        ColdBonus = 25;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GauntletsOfTheWild(Serial serial) : base(serial)
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
