using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PickpocketsNimbleGloves : StrawHat
{
    [Constructable]
    public PickpocketsNimbleGloves()
    {
        Name = "Pickpocket's Nimble Hat";
        Hue = Utility.Random(800, 2700);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 5;
        SkillBonuses.SetValues(0, SkillName.Stealing, 25.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
        Resistances.Physical = 10;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PickpocketsNimbleGloves(Serial serial) : base(serial)
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
