using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DancersEnchantedSkirt : Kilt
{
    [Constructable]
    public DancersEnchantedSkirt()
    {
        Name = "Dancer's Enchanted Skirt";
        Hue = Utility.Random(50, 2400);
        Attributes.BonusDex = 20;
        Attributes.WeaponSpeed = 5;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DancersEnchantedSkirt(Serial serial) : base(serial)
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
