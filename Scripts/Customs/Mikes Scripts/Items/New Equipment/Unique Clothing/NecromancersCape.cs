using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersCape : Cloak
{
    [Constructable]
    public NecromancersCape()
    {
        Name = "Necromancer's Cape";
        Hue = Utility.Random(500, 1500);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusInt = 20;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);
        Resistances.Cold = 15;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersCape(Serial serial) : base(serial)
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
