using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BaristasMuffler : Cap
{
    [Constructable]
    public BaristasMuffler()
    {
        Name = "Barista's Muffler";
        Hue = Utility.Random(350, 2850);
        Attributes.BonusInt = 6;
        Attributes.RegenMana = 2;
        SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
        Resistances.Fire = 15;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BaristasMuffler(Serial serial) : base(serial)
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
