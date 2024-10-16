using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoguesDeceptiveMask : BearMask
{
    [Constructable]
    public RoguesDeceptiveMask()
    {
        Name = "Rogue's Deceptive Mask";
        Hue = Utility.Random(900, 2600);
        Attributes.BonusDex = 5;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
        Resistances.Energy = 15;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoguesDeceptiveMask(Serial serial) : base(serial)
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
