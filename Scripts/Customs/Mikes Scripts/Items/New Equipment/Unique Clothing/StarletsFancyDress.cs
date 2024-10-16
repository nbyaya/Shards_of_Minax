using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarletsFancyDress : PlainDress
{
    [Constructable]
    public StarletsFancyDress()
    {
        Name = "Starlet's Fancy Dress";
        Hue = Utility.Random(500, 2000);
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        Resistances.Energy = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarletsFancyDress(Serial serial) : base(serial)
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
