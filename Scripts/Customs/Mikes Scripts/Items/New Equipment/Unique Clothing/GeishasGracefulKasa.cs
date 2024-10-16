using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GeishasGracefulKasa : Kasa
{
    [Constructable]
    public GeishasGracefulKasa()
    {
        Name = "Geisha's Graceful Kasa";
        Hue = Utility.Random(1, 2900);
        Attributes.EnhancePotions = 10;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GeishasGracefulKasa(Serial serial) : base(serial)
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
