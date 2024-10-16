using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeggarsBondLeggings : LeatherLegs
{
    [Constructable]
    public BeggarsBondLeggings()
    {
        Name = "Beggar's Bond Leggings";
        Hue = Utility.Random(300, 450);
        BaseArmorRating = Utility.RandomMinMax(55, 80);
        Attributes.BonusDex = 25;
        Attributes.BonusStam = 20;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Begging, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 30.0);
        PhysicalBonus = 18;
        FireBonus = 12;
        ColdBonus = 22;
        EnergyBonus = 18;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeggarsBondLeggings(Serial serial) : base(serial)
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
