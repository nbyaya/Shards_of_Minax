using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CyberpunkNinjaTabi : NinjaTabi
{
    [Constructable]
    public CyberpunkNinjaTabi()
    {
        Name = "Cyberpunk NinjaTabi";
        Hue = Utility.Random(750, 2700);
        Attributes.BonusDex = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
        Resistances.Energy = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CyberpunkNinjaTabi(Serial serial) : base(serial)
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
