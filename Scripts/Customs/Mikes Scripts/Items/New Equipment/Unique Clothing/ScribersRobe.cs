using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScribersRobe : Robe
{
    [Constructable]
    public ScribersRobe()
    {
        Name = "Scribe's Robe";
        Hue = Utility.Random(200, 2200);
        ClothingAttributes.MageArmor = 1;
        Attributes.BonusInt = 15;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 20.0);
        Resistances.Energy = 15;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScribersRobe(Serial serial) : base(serial)
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
