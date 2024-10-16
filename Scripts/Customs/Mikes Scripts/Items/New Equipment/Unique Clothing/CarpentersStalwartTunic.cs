using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CarpentersStalwartTunic : Tunic
{
    [Constructable]
    public CarpentersStalwartTunic()
    {
        Name = "Carpenter's Stalwart Tunic";
        Hue = Utility.Random(700, 1650);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 10;
        Attributes.DefendChance = 5;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 25.0);
        Resistances.Physical = 20;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CarpentersStalwartTunic(Serial serial) : base(serial)
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
