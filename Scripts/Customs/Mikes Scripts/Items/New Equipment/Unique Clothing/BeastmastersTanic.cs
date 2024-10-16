using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersTanic : Tunic
{
    [Constructable]
    public BeastmastersTanic()
    {
        Name = "Beastmaster's Tunic";
        Hue = Utility.Random(700, 1700);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 20.0);
        Resistances.Physical = 15;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersTanic(Serial serial) : base(serial)
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
