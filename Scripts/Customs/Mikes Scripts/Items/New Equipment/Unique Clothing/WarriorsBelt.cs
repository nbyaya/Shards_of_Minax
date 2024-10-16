using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarriorsBelt : BodySash
{
    [Constructable]
    public WarriorsBelt()
    {
        Name = "Warrior's Belt";
        Hue = Utility.Random(500, 2500);
        Attributes.BonusHits = 15;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarriorsBelt(Serial serial) : base(serial)
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
