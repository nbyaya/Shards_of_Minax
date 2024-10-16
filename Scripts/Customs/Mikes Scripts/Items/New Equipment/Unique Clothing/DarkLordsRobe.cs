using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkLordsRobe : Robe
{
    [Constructable]
    public DarkLordsRobe()
    {
        Name = "Dark Lord's Robe";
        Hue = Utility.Random(1, 1908);
        ClothingAttributes.SelfRepair = 5;
        Attributes.BonusDex = 50;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        Resistances.Physical = 10;
        Resistances.Fire = 10;
        Resistances.Cold = 10;
        Resistances.Poison = 10;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkLordsRobe(Serial serial) : base(serial)
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
