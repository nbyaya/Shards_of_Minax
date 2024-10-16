using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SneaksSilentShoes : Shoes
{
    [Constructable]
    public SneaksSilentShoes()
    {
        Name = "Sneak's Silent Shoes";
        Hue = Utility.Random(1000, 2800);
        Attributes.BonusDex = 10;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 15.0);
        Resistances.Physical = 10;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SneaksSilentShoes(Serial serial) : base(serial)
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
