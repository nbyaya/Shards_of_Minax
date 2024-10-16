using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RangersHatNightSight : WizardsHat
{
    [Constructable]
    public RangersHatNightSight()
    {
        Name = "Ranger's Hat";
        Hue = Utility.Random(100, 2100);
        Attributes.BonusStr = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        Resistances.Energy = 5;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RangersHatNightSight(Serial serial) : base(serial)
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
