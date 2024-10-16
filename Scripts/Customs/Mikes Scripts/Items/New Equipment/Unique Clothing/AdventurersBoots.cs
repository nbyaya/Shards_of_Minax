using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AdventurersBoots : Boots
{
    [Constructable]
    public AdventurersBoots()
    {
        Name = "Adventurer's Boots";
        Hue = Utility.Random(250, 2150);
        ClothingAttributes.LowerStatReq = 2;
        Attributes.BonusStam = 10;
        SkillBonuses.SetValues(0, SkillName.Camping, 20.0);
        Resistances.Physical = 15;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AdventurersBoots(Serial serial) : base(serial)
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
