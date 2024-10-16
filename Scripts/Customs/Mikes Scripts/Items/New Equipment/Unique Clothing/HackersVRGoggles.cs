using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HackersVRGoggles : FeatheredHat
{
    [Constructable]
    public HackersVRGoggles()
    {
        Name = "Hacker's VR Goggles";
        Hue = Utility.Random(900, 2800);
        Attributes.AttackChance = 15;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 75.0);
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
        // Note: Chaos resistance does not exist in ServUO; will ignore it
    }

    public HackersVRGoggles(Serial serial) : base(serial)
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
