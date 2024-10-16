using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArtisansTimberShoes : Shoes
{
    [Constructable]
    public ArtisansTimberShoes()
    {
        Name = "Artisan's Timber Shoes";
        Hue = Utility.Random(650, 1600);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusStr = 5;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0);
        Resistances.Energy = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArtisansTimberShoes(Serial serial) : base(serial)
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
