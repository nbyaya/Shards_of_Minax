using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MonksMeditativeRobe : Robe
{
    [Constructable]
    public MonksMeditativeRobe()
    {
        Name = "Monk's Meditative Robe";
        Hue = Utility.Random(200, 2200);
        Attributes.BonusInt = 15;
        SkillBonuses.SetValues(0, SkillName.Meditation, 25.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
        Resistances.Fire = 10;  // Note: If "Chaotic" is not a standard attribute, you might need to use an alternate or custom implementation.
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MonksMeditativeRobe(Serial serial) : base(serial)
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
