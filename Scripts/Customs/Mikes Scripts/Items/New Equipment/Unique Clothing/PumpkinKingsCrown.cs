using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PumpkinKingsCrown : WideBrimHat
{
    [Constructable]
    public PumpkinKingsCrown()
    {
        Name = "Pumpkin King's Crown";
        Hue = Utility.Random(35, 2938);
        Attributes.BonusStr = 10;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
        SkillBonuses.SetValues(1, SkillName.Cooking, 20.0);
        Resistances.Fire = 15;
        Resistances.Cold = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PumpkinKingsCrown(Serial serial) : base(serial)
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
