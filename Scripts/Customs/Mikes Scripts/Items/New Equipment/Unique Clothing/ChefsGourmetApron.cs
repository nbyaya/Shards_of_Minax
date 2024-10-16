using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChefsGourmetApron : FullApron
{
    [Constructable]
    public ChefsGourmetApron()
    {
        Name = "Chef's Gourmet Apron";
        Hue = Utility.Random(250, 2750);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusInt = 5;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
        Resistances.Fire = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChefsGourmetApron(Serial serial) : base(serial)
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
