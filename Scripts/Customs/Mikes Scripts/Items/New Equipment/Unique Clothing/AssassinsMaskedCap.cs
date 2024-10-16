using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AssassinsMaskedCap : SkullCap
{
    [Constructable]
    public AssassinsMaskedCap()
    {
        Name = "Assassin's Masked Cap";
        Hue = Utility.Random(1, 2000);
        Attributes.NightSight = 1;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AssassinsMaskedCap(Serial serial) : base(serial)
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
