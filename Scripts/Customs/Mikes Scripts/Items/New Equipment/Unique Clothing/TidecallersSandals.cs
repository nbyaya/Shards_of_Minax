using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TidecallersSandals : Sandals
{
    [Constructable]
    public TidecallersSandals()
    {
        Name = "Tidecaller's Sandals";
        Hue = Utility.Random(350, 2900);
        Attributes.SpellDamage = 5;
        Attributes.BonusDex = 7;
        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
        Resistances.Fire = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TidecallersSandals(Serial serial) : base(serial)
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
