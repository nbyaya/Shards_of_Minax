using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BardsTunicOfStonehenge : Tunic
{
    [Constructable]
    public BardsTunicOfStonehenge()
    {
        Name = "Bard's Tunic of Stonehenge";
        Hue = Utility.Random(250, 2250);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 5;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BardsTunicOfStonehenge(Serial serial) : base(serial)
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
