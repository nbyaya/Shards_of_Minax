using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ForestersTunic : Tunic
{
    [Constructable]
    public ForestersTunic()
    {
        Name = "Forester's Tunic";
        Hue = Utility.Random(150, 2000);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Camping, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ForestersTunic(Serial serial) : base(serial)
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
