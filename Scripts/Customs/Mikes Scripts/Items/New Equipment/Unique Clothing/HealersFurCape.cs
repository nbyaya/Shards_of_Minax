using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HealersFurCape : FurCape
{
    [Constructable]
    public HealersFurCape()
    {
        Name = "Healer's Fur Cape";
        Hue = Utility.Random(500, 2500);
        ClothingAttributes.LowerStatReq = 2;
        Attributes.BonusStr = 5;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Veterinary, 25.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
        Resistances.Physical = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HealersFurCape(Serial serial) : base(serial)
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
