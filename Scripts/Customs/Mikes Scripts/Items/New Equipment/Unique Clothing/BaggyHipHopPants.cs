using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BaggyHipHopPants : LongPants
{
    [Constructable]
    public BaggyHipHopPants()
    {
        Name = "Baggy Hip-Hop Pants";
        Hue = Utility.Random(700, 2700);
        Attributes.BonusStam = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 10.0);
        Resistances.Physical = 10;
        Resistances.Poison = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BaggyHipHopPants(Serial serial) : base(serial)
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
