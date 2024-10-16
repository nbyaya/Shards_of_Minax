using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShogunsAuthoritativeSurcoat : Surcoat
{
    [Constructable]
    public ShogunsAuthoritativeSurcoat()
    {
        Name = "Shogun's Authoritative Surcoat";
        Hue = Utility.Random(500, 2500);
        Attributes.BonusStr = 10;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        Resistances.Physical = 15;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShogunsAuthoritativeSurcoat(Serial serial) : base(serial)
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
