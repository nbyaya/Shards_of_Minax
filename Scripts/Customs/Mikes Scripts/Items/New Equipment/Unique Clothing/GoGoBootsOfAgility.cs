using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GoGoBootsOfAgility : ThighBoots
{
    [Constructable]
    public GoGoBootsOfAgility()
    {
        Name = "Go-Go Boots of Agility";
        Hue = Utility.Random(950, 2980);
        Attributes.BonusDex = 20;
        Attributes.WeaponSpeed = 5;
        ClothingAttributes.LowerStatReq = 3;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        Resistances.Energy = 20;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GoGoBootsOfAgility(Serial serial) : base(serial)
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
