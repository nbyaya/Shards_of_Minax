using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LeprechaunsLuckyHat : FeatheredHat
{
    [Constructable]
    public LeprechaunsLuckyHat()
    {
        Name = "Leprechaun's Lucky Hat";
        Hue = Utility.Random(600, 2650);
        ClothingAttributes.SelfRepair = 5;
        Attributes.Luck = 50;
        SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 15.0);
        Resistances.Physical = 10;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LeprechaunsLuckyHat(Serial serial) : base(serial)
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
