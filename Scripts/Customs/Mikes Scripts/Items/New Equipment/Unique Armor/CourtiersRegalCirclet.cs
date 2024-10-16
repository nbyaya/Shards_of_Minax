using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtiersRegalCirclet : PlateHelm
{
    [Constructable]
    public CourtiersRegalCirclet()
    {
        Name = "Courtier's Regal Circlet";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(35, 60);
        AbsorptionAttributes.EaterCold = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusInt = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtiersRegalCirclet(Serial serial) : base(serial)
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
