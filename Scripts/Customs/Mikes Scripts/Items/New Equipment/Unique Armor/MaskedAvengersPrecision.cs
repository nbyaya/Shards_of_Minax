using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskedAvengersPrecision : StuddedGloves
{
    [Constructable]
    public MaskedAvengersPrecision()
    {
        Name = "Masked Avenger's Precision";
        Hue = Utility.Random(900, 999);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskedAvengersPrecision(Serial serial) : base(serial)
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
