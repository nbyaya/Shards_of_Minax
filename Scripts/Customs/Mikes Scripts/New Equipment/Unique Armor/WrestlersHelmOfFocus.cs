using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WrestlersHelmOfFocus : NorseHelm
{
    [Constructable]
    public WrestlersHelmOfFocus()
    {
        Name = "Wrestler's Helm of Focus";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(40, 65);
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WrestlersHelmOfFocus(Serial serial) : base(serial)
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
