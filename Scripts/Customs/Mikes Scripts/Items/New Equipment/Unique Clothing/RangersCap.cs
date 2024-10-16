using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RangersCap : Cap
{
    [Constructable]
    public RangersCap()
    {
        Name = "Ranger's Cap";
        Hue = Utility.Random(850, 1850);
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        Resistances.Energy = 15;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RangersCap(Serial serial) : base(serial)
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
