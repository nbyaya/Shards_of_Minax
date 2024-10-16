using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PreserversCap : Cap
{
    [Constructable]
    public PreserversCap()
    {
        Name = "Preserver's Cap";
        Hue = Utility.Random(350, 2350);
        ClothingAttributes.ReactiveParalyze = 2;
        Attributes.BonusStr = 5;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 15.0);
        SkillBonuses.SetValues(1, SkillName.Carpentry, 10.0);
        Resistances.Fire = 5;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PreserversCap(Serial serial) : base(serial)
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
