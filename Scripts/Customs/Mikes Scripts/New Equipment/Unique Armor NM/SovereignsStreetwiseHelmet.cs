using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SovereignsStreetwiseHelmet : PlateHelm
{
    [Constructable]
    public SovereignsStreetwiseHelmet()
    {
        Name = "Sovereign's Streetwise Helmet";
        Hue = Utility.Random(50, 150);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        ArmorAttributes.MageArmor = 1;
        Attributes.NightSight = 1;
        Attributes.ReflectPhysical = 20;
        SkillBonuses.SetValues(0, SkillName.Begging, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 35.0);
        PhysicalBonus = 10;
        FireBonus = 25;
        ColdBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SovereignsStreetwiseHelmet(Serial serial) : base(serial)
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
