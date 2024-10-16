using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrapplersTunic : Tunic
{
    [Constructable]
    public GrapplersTunic()
    {
        Name = "Grappler's Tunic";
        Hue = Utility.Random(100, 2100);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 25.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrapplersTunic(Serial serial) : base(serial)
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
