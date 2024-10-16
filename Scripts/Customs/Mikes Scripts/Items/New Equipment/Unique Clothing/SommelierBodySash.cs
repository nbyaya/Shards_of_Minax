using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SommelierBodySash : BodySash
{
    [Constructable]
    public SommelierBodySash()
    {
        Name = "Sommelier's Body Sash";
        Hue = Utility.Random(400, 2200);
        Attributes.BonusInt = 7;
        Attributes.Luck = 10;
        SkillBonuses.SetValues(0, SkillName.TasteID, 20.0);
        Resistances.Cold = 10;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SommelierBodySash(Serial serial) : base(serial)
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
