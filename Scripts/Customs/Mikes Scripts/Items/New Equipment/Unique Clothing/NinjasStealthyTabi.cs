using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NinjasStealthyTabi : NinjaTabi
{
    [Constructable]
    public NinjasStealthyTabi()
    {
        Name = "Ninja's Stealthy Tabi";
        Hue = Utility.Random(800, 2800);
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NinjasStealthyTabi(Serial serial) : base(serial)
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
