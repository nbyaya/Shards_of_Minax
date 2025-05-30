using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HerbalistsProtectiveHat : StrawHat
{
    [Constructable]
    public HerbalistsProtectiveHat()
    {
        Name = "Herbalist's Protective Hat";
        Hue = Utility.Random(280, 2280);
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        Resistances.Poison = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HerbalistsProtectiveHat(Serial serial) : base(serial)
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
