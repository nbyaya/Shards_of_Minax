using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DapperFedoraOfInsight : WizardsHat
{
    [Constructable]
    public DapperFedoraOfInsight()
    {
        Name = "Dapper Fedora of Insight";
        Hue = Utility.Random(300, 2100);
        Attributes.BonusInt = 12;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DapperFedoraOfInsight(Serial serial) : base(serial)
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
