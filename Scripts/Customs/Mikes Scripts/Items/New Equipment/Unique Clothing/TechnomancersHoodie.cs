using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TechnomancersHoodie : Robe
{
    [Constructable]
    public TechnomancersHoodie()
    {
        Name = "Technomancer's Hoodie";
        Hue = Utility.Random(800, 2900);
        ClothingAttributes.MageArmor = 1;
        Attributes.CastSpeed = 10;
        Attributes.BonusInt = 20;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        Resistances.Energy = 20;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TechnomancersHoodie(Serial serial) : base(serial)
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
