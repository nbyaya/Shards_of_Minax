using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class QuivermastersTunic : Tunic
{
    [Constructable]
    public QuivermastersTunic()
    {
        Name = "Quivermaster's Tunic";
        Hue = Utility.Random(500, 2500);
        ClothingAttributes.DurabilityBonus = 3;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 10.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public QuivermastersTunic(Serial serial) : base(serial)
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
