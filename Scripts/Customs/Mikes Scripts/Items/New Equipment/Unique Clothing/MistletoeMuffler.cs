using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MistletoeMuffler : Cap
{
    [Constructable]
    public MistletoeMuffler()
    {
        Name = "Mistletoe Muffler";
        Hue = Utility.Random(60, 2970);
        Attributes.BonusDex = 8;
        Attributes.BonusStr = 10; // Assuming BonusCha is a valid attribute
        SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
        Resistances.Cold = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MistletoeMuffler(Serial serial) : base(serial)
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
