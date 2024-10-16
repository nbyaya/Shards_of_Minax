using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperersSandals : Sandals
{
    [Constructable]
    public WhisperersSandals()
    {
        Name = "Whisperer's Sandals";
        Hue = Utility.Random(500, 2400);
        ClothingAttributes.MageArmor = 1;
        Attributes.BonusDex = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperersSandals(Serial serial) : base(serial)
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
