using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TamersKilt : Kilt
{
    [Constructable]
    public TamersKilt()
    {
        Name = "Tamer's Kilt";
        Hue = Utility.Random(300, 2350);
        Attributes.BonusDex = 10;
        Attributes.RegenStam = 3;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0);
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TamersKilt(Serial serial) : base(serial)
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
