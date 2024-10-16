using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DiscoDivaBoots : ThighBoots
{
    [Constructable]
    public DiscoDivaBoots()
    {
        Name = "Disco Diva Boots";
        Hue = Utility.Random(200, 2999);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusDex = 15;
        Attributes.Luck = 10;
        SkillBonuses.SetValues(0, SkillName.Provocation, 15.0);
        Resistances.Energy = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DiscoDivaBoots(Serial serial) : base(serial)
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
