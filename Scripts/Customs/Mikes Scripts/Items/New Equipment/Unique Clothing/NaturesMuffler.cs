using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaturesMuffler : Cap
{
    [Constructable]
    public NaturesMuffler()
    {
        Name = "Nature's Muffler";
        Hue = Utility.Random(350, 2350);
        ClothingAttributes.DurabilityBonus = 3;
        Attributes.BonusDex = 10;
        Attributes.DefendChance = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
        Resistances.Cold = 15;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaturesMuffler(Serial serial) : base(serial)
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
