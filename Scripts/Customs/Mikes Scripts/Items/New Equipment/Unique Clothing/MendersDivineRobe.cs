using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MendersDivineRobe : Robe
{
    [Constructable]
    public MendersDivineRobe()
    {
        Name = "Mender's Divine Robe";
        Hue = Utility.Random(200, 2200);
        Attributes.BonusInt = 15;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Healing, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        Resistances.Energy = 20;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MendersDivineRobe(Serial serial) : base(serial)
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
