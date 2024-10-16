using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HealersBlessedSandals : Sandals
{
    [Constructable]
    public HealersBlessedSandals()
    {
        Name = "Healer's Blessed Sandals";
        Hue = Utility.Random(250, 2250);
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        Resistances.Poison = 20;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HealersBlessedSandals(Serial serial) : base(serial)
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
