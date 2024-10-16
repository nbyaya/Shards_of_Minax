using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VampiresMidnightCloak : Cloak
{
    [Constructable]
    public VampiresMidnightCloak()
    {
        Name = "Vampire's Midnight Cloak";
        Hue = Utility.Random(490, 2600);
        Attributes.NightSight = 1;
        Attributes.BonusDex = 15;
        SkillBonuses.SetValues(0, SkillName.Stealth, 30.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);
        Resistances.Cold = 20;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VampiresMidnightCloak(Serial serial) : base(serial)
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
