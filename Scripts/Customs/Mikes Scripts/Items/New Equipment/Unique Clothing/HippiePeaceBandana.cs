using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HippiePeaceBandana : Bandana
{
    [Constructable]
    public HippiePeaceBandana()
    {
        Name = "Hippie Peace Bandana";
        Hue = Utility.Random(300, 2300);
        Attributes.BonusMana = 10;
        Attributes.LowerRegCost = 5;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HippiePeaceBandana(Serial serial) : base(serial)
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
