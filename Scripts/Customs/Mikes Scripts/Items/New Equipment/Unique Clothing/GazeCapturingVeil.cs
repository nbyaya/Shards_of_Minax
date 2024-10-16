using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GazeCapturingVeil : BodySash
{
    [Constructable]
    public GazeCapturingVeil()
    {
        Name = "Gaze-Capturing Veil";
        Hue = Utility.Random(200, 2300);
        Attributes.BonusInt = 10;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
        Resistances.Cold = 5;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GazeCapturingVeil(Serial serial) : base(serial)
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
