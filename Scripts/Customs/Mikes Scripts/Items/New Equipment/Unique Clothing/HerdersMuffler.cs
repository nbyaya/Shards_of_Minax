using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HerdersMuffler : Cap
{
    [Constructable]
    public HerdersMuffler()
    {
        Name = "Herder's Muffler";
        Hue = Utility.Random(400, 2300);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Herding, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
        Resistances.Cold = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HerdersMuffler(Serial serial) : base(serial)
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
