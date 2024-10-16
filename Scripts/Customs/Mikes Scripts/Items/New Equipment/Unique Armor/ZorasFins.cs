using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ZorasFins : LeatherLegs
{
    [Constructable]
    public ZorasFins()
    {
        Name = "Zora's Fins";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.SelfRepair = 10;
        Attributes.RegenStam = 10;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ZorasFins(Serial serial) : base(serial)
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
