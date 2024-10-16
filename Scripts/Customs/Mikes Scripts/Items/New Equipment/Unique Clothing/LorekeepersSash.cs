using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LorekeepersSash : BodySash
{
    [Constructable]
    public LorekeepersSash()
    {
        Name = "Lorekeeper's Sash";
        Hue = Utility.Random(400, 2400);
        ClothingAttributes.MageArmor = 1;
        Attributes.BonusMana = 10;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        Resistances.Cold = 10;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LorekeepersSash(Serial serial) : base(serial)
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
