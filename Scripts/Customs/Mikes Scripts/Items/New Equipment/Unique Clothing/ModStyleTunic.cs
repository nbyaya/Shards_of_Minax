using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ModStyleTunic : Tunic
{
    [Constructable]
    public ModStyleTunic()
    {
        Name = "Mod-Style Tunic";
        Hue = Utility.Random(250, 2750);
        Attributes.BonusDex = 10;
        Attributes.CastSpeed = 1;
        ClothingAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
        Resistances.Energy = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ModStyleTunic(Serial serial) : base(serial)
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
