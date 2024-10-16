using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SawyersMightyApron : FullApron
{
    [Constructable]
    public SawyersMightyApron()
    {
        Name = "Sawyer's Mighty Apron";
        Hue = Utility.Random(730, 1690);
        Attributes.BonusStr = 10;
        Attributes.RegenHits = 2;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
        Resistances.Physical = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SawyersMightyApron(Serial serial) : base(serial)
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
