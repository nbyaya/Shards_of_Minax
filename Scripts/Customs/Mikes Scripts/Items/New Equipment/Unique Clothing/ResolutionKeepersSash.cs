using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ResolutionKeepersSash : BodySash
{
    [Constructable]
    public ResolutionKeepersSash()
    {
        Name = "Resolution Keeper's Sash";
        Hue = Utility.Random(150, 2850);
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
		SkillBonuses.SetValues(0, SkillName.Focus, 35.0);
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ResolutionKeepersSash(Serial serial) : base(serial)
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
