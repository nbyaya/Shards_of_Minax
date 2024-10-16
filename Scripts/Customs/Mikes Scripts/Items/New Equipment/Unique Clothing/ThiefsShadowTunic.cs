using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThiefsShadowTunic : Tunic
{
    [Constructable]
    public ThiefsShadowTunic()
    {
        Name = "Thief's Shadow Tunic";
        Hue = Utility.Random(1100, 2900);
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealing, 25.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        Resistances.Physical = 20;
        Resistances.Cold = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThiefsShadowTunic(Serial serial) : base(serial)
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
