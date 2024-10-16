using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MummysWrappings : BodySash
{
    [Constructable]
    public MummysWrappings()
    {
        Name = "Mummy's Wrappings";
        Hue = Utility.Random(650, 2750);
        Attributes.DefendChance = 15;
        Attributes.LowerRegCost = 10;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);
        Resistances.Physical = 25;
        Resistances.Poison = 30;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MummysWrappings(Serial serial) : base(serial)
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
