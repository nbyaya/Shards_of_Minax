using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AssassinsBandana : Bandana
{
    [Constructable]
    public AssassinsBandana()
    {
        Name = "Assassin's Bandana";
        Hue = Utility.Random(1, 1000);
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
        SkillBonuses.SetValues(1, SkillName.Fencing, 15.0);
        Resistances.Energy = 10;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AssassinsBandana(Serial serial) : base(serial)
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
