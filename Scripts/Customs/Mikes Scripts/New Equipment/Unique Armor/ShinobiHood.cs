using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShinobiHood : NorseHelm
{
    [Constructable]
    public ShinobiHood()
    {
        Name = "Shinobi Hood";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusInt = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShinobiHood(Serial serial) : base(serial)
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
