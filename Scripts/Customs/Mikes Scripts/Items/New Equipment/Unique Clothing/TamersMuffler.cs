using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TamersMuffler : Cap
{
    [Constructable]
    public TamersMuffler()
    {
        Name = "Tamer's Muffler";
        Hue = Utility.Random(250, 2300);
        ClothingAttributes.SelfRepair = 3;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
        Resistances.Poison = 15;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TamersMuffler(Serial serial) : base(serial)
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
