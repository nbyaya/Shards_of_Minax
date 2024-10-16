using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GhostlyShroud : Robe
{
    [Constructable]
    public GhostlyShroud()
    {
        Name = "Ghostly Shroud";
        Hue = Utility.Random(800, 2900);
        ClothingAttributes.MageArmor = 1;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 30.0);
        Resistances.Cold = 25;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GhostlyShroud(Serial serial) : base(serial)
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
