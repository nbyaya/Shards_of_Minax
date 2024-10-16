using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LuchadorsMask : TribalMask
{
    [Constructable]
    public LuchadorsMask()
    {
        Name = "Luchador's Mask";
        Hue = Utility.Random(10, 2000);
        Attributes.BonusDex = 15;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        Resistances.Physical = 15;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LuchadorsMask(Serial serial) : base(serial)
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
