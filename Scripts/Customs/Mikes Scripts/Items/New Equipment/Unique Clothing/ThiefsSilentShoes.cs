using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThiefsSilentShoes : Shoes
{
    [Constructable]
    public ThiefsSilentShoes()
    {
        Name = "Thief's Silent Shoes";
        Hue = Utility.Random(900, 2700);
        ClothingAttributes.LowerStatReq = 2;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThiefsSilentShoes(Serial serial) : base(serial)
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
