using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CouturiersSundress : PlainDress
{
    [Constructable]
    public CouturiersSundress()
    {
        Name = "Couturier's Sundress";
        Hue = Utility.Random(100, 2600);
        Attributes.EnhancePotions = 15;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 20.0);
        Resistances.Fire = 10;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CouturiersSundress(Serial serial) : base(serial)
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
