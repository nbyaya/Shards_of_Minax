using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NewWaveNeonShades : BodySash
{
    [Constructable]
    public NewWaveNeonShades()
    {
        Name = "New Wave Neon Shades";
        Hue = Utility.Random(700, 2700);
        Attributes.BonusInt = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
        Resistances.Energy = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NewWaveNeonShades(Serial serial) : base(serial)
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
