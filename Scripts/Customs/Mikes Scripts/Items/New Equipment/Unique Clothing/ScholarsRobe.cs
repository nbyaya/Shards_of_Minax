using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScholarsRobe : Robe
{
    [Constructable]
    public ScholarsRobe()
    {
        Name = "Scholar's Robe";
        Hue = Utility.Random(200, 2200);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusInt = 15;
        Attributes.LowerManaCost = 8;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        Resistances.Energy = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScholarsRobe(Serial serial) : base(serial)
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
