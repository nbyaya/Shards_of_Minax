using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelesRunebladeBuckler : HeaterShield
{
    [Constructable]
    public CelesRunebladeBuckler()
    {
        Name = "Celes' Runeblade Buckler";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        Attributes.ReflectPhysical = 5;
        Attributes.BonusInt = 15;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelesRunebladeBuckler(Serial serial) : base(serial)
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
