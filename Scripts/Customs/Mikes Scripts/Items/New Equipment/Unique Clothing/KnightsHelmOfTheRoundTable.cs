using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnightsHelmOfTheRoundTable : StrawHat
{
    [Constructable]
    public KnightsHelmOfTheRoundTable()
    {
        Name = "Knight's Hat of the Round Table";
        Hue = Utility.Random(400, 2400);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusStr = 10;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        Resistances.Physical = 25;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnightsHelmOfTheRoundTable(Serial serial) : base(serial)
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
