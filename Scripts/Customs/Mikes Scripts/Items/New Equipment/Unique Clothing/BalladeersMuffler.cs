using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BalladeersMuffler : Cap
{
    [Constructable]
    public BalladeersMuffler()
    {
        Name = "Balladeer's Muffler";
        Hue = Utility.Random(250, 2250);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusInt = 7;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 10.0);
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BalladeersMuffler(Serial serial) : base(serial)
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
