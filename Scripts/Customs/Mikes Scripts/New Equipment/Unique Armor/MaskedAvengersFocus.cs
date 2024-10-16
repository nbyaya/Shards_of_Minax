using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskedAvengersFocus : LeatherCap
{
    [Constructable]
    public MaskedAvengersFocus()
    {
        Name = "Masked Avenger's Focus";
        Hue = Utility.Random(900, 999);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        Attributes.CastSpeed = 1;
        Attributes.BonusMana = 10;
        SkillBonuses.SetValues(0, SkillName.Focus, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskedAvengersFocus(Serial serial) : base(serial)
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
