using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShepherdsKilt : Kilt
{
    [Constructable]
    public ShepherdsKilt()
    {
        Name = "Shepherd's Kilt";
        Hue = Utility.Random(800, 1800);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Herding, 25.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
        Resistances.Cold = 10;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShepherdsKilt(Serial serial) : base(serial)
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
