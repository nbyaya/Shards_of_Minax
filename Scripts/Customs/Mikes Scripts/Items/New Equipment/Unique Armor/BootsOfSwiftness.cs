using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BootsOfSwiftness : LeatherLegs
{
    [Constructable]
    public BootsOfSwiftness()
    {
        Name = "Boots of Swiftness";
        Hue = Utility.Random(250, 650);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 25;
        Attributes.RegenStam = 7;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        PhysicalBonus = 15;
        EnergyBonus = 5;
        FireBonus = 5;
        PoisonBonus = 10;
        ColdBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BootsOfSwiftness(Serial serial) : base(serial)
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
