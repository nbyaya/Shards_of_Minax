using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonistsSoftShoes : Shoes
{
    [Constructable]
    public HarmonistsSoftShoes()
    {
        Name = "Harmonist's Soft Shoes";
        Hue = Utility.Random(400, 2400);
        ClothingAttributes.LowerStatReq = 2;
        Attributes.BonusStam = 5;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonistsSoftShoes(Serial serial) : base(serial)
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
