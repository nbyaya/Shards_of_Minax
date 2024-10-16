using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SwingsDancersShoes : Shoes
{
    [Constructable]
    public SwingsDancersShoes()
    {
        Name = "Swing Dancer's Shoes";
        Hue = Utility.Random(100, 2500);
        Attributes.BonusDex = 10;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
        Resistances.Energy = 10;
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SwingsDancersShoes(Serial serial) : base(serial)
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
