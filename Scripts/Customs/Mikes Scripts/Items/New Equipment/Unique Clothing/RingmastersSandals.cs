using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RingmastersSandals : Sandals
{
    [Constructable]
    public RingmastersSandals()
    {
        Name = "Ringmaster's Sandals";
        Hue = Utility.Random(400, 2400);
        Attributes.BonusStam = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RingmastersSandals(Serial serial) : base(serial)
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
