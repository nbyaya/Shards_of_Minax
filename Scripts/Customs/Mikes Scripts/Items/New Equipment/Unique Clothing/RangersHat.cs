using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RangersHat : WizardsHat
{
    [Constructable]
    public RangersHat()
    {
        Name = "Ranger's Hat";
        Hue = Utility.Random(100, 1100);
        ClothingAttributes.ReactiveParalyze = 1;
        Attributes.BonusInt = 10;
        Attributes.LowerRegCost = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
        Resistances.Physical = 15;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RangersHat(Serial serial) : base(serial)
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
