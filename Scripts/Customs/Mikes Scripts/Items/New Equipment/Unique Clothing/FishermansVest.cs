using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FishermansVest : BodySash
{
    [Constructable]
    public FishermansVest()
    {
        Name = "Fisherman's Vest";
        Hue = Utility.Random(400, 2300);
        Attributes.BonusInt = 8;
        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);
        Resistances.Fire = 10; // Note: Water resistance may need to be adjusted if this is not a standard attribute.
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FishermansVest(Serial serial) : base(serial)
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
