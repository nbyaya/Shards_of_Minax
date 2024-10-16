using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersTonic : Tunic
{
    [Constructable]
    public BeastmastersTonic()
    {
        Name = "Beastmaster's Tunic";
        Hue = Utility.Random(450, 2450);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
        Resistances.Physical = 10;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersTonic(Serial serial) : base(serial)
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
