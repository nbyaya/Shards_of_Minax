using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DeepSeaTunic : Tunic
{
    [Constructable]
    public DeepSeaTunic()
    {
        Name = "Deep Sea Tunic";
        Hue = Utility.Random(500, 2500);
        Attributes.BonusMana = 10;
        SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
		SkillBonuses.SetValues(0, SkillName.DetectHidden, 25.0);
        Resistances.Fire = 20;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DeepSeaTunic(Serial serial) : base(serial)
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
