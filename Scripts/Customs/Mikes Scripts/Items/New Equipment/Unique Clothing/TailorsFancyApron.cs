using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TailorsFancyApron : FullApron
{
    [Constructable]
    public TailorsFancyApron()
    {
        Name = "Tailor's Fancy Apron";
        Hue = Utility.Random(300, 1200);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusDex = 10;
        Attributes.RegenMana = 2;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 20.0);
        Resistances.Cold = 5;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TailorsFancyApron(Serial serial) : base(serial)
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
