using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CropTopMystic : FancyShirt
{
    [Constructable]
    public CropTopMystic()
    {
        Name = "Crop Top Mystic";
        Hue = Utility.Random(50, 2950);
        Attributes.BonusInt = 10;
        Attributes.SpellDamage = 8;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 12.0);
        Resistances.Energy = 10;
        Resistances.Cold = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CropTopMystic(Serial serial) : base(serial)
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
