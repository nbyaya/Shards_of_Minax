using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeductressSilkenShoes : Shoes
{
    [Constructable]
    public SeductressSilkenShoes()
    {
        Name = "Seductress' Silken Shoes";
        Hue = Utility.Random(400, 2400);
        Attributes.BonusDex = 5;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        Resistances.Cold = 5;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeductressSilkenShoes(Serial serial) : base(serial)
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
