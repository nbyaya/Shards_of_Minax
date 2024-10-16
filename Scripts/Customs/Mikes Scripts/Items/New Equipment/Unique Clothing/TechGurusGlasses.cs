using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TechGurusGlasses : Bandana
{
    [Constructable]
    public TechGurusGlasses()
    {
        Name = "Tech Guru's Bandana";
        Hue = Utility.Random(1, 2950);
        Attributes.BonusInt = 15;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 15.0);
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TechGurusGlasses(Serial serial) : base(serial)
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
