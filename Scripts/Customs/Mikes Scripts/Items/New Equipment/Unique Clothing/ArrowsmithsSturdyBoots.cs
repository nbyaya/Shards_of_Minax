using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArrowsmithsSturdyBoots : Boots
{
    [Constructable]
    public ArrowsmithsSturdyBoots()
    {
        Name = "Arrowsmith's Sturdy Boots";
        Hue = Utility.Random(600, 2600);
        Attributes.WeaponSpeed = 5;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
        Resistances.Physical = 15;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArrowsmithsSturdyBoots(Serial serial) : base(serial)
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
