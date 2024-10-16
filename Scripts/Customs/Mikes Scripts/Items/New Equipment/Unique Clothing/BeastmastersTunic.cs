using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersTunic : Tunic
{
    [Constructable]
    public BeastmastersTunic()
    {
        Name = "Beastmaster's Tunic";
        Hue = Utility.Random(250, 2250);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 25.0);
        Resistances.Physical = 10;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersTunic(Serial serial) : base(serial)
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
