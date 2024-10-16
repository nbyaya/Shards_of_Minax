using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingSandals : Sandals
{
    [Constructable]
    public WhisperingSandals()
    {
        Name = "Whispering Sandals";
        Hue = Utility.Random(400, 2400);
        ClothingAttributes.MageArmor = 1;
        Attributes.BonusDex = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
        Resistances.Cold = 5;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingSandals(Serial serial) : base(serial)
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
