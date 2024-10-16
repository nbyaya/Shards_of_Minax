using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScoutsWideBrimHat : WideBrimHat
{
    [Constructable]
    public ScoutsWideBrimHat()
    {
        Name = "Scout's Wide Brim Hat";
        Hue = Utility.Random(350, 2200);
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
        SkillBonuses.SetValues(1, SkillName.Camping, 15.0);
        Resistances.Energy = 10;
        Resistances.Cold = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScoutsWideBrimHat(Serial serial) : base(serial)
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
