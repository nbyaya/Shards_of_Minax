using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HippiesPeacefulSandals : Sandals
{
    [Constructable]
    public HippiesPeacefulSandals()
    {
        Name = "Hippie's Peaceful Sandals";
        Hue = Utility.Random(500, 2200);
        Attributes.RegenMana = 3;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 10.0);
        Resistances.Cold = 5;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HippiesPeacefulSandals(Serial serial) : base(serial)
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
