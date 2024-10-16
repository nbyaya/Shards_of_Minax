using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoyalCircletHelm : NorseHelm
{
    [Constructable]
    public RoyalCircletHelm()
    {
        Name = "Royal Circlet Helm";
        Hue = Utility.Random(300, 900);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        Attributes.BonusStam = 20;
        Attributes.RegenMana = 5;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoyalCircletHelm(Serial serial) : base(serial)
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
