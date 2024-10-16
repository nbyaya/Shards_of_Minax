using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PickpocketsSleekTunic : Tunic
{
    [Constructable]
    public PickpocketsSleekTunic()
    {
        Name = "Pickpocket's Sleek Tunic";
        Hue = Utility.Random(300, 2200);
        ClothingAttributes.SelfRepair = 2;
        Attributes.BonusDex = 15;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 25.0);
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PickpocketsSleekTunic(Serial serial) : base(serial)
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
