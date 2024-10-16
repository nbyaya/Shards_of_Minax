using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SurgeonsInsightfulMask : BearMask
{
    [Constructable]
    public SurgeonsInsightfulMask()
    {
        Name = "Surgeon's Insightful Mask";
        Hue = Utility.Random(240, 2240);
        Attributes.BonusInt = 10;
        Attributes.RegenHits = 3;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        Resistances.Poison = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SurgeonsInsightfulMask(Serial serial) : base(serial)
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
