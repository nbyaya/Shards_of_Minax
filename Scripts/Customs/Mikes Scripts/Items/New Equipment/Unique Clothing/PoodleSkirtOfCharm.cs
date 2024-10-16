using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PoodleSkirtOfCharm : Kilt
{
    [Constructable]
    public PoodleSkirtOfCharm()
    {
        Name = "Poodle Skirt of Charm";
        Hue = Utility.Random(200, 2200);
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PoodleSkirtOfCharm(Serial serial) : base(serial)
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
