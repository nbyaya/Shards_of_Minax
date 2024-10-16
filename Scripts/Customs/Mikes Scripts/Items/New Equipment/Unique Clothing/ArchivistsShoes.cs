using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArchivistsShoes : Shoes
{
    [Constructable]
    public ArchivistsShoes()
    {
        Name = "Archivist's Shoes";
        Hue = Utility.Random(300, 2300);
        Attributes.NightSight = 1;
        Attributes.LowerRegCost = 7;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
        Resistances.Energy = 10;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArchivistsShoes(Serial serial) : base(serial)
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
