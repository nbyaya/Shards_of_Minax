using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CuratorsKilt : Kilt
{
    [Constructable]
    public CuratorsKilt()
    {
        Name = "Curator's Kilt";
        Hue = Utility.Random(250, 2250);
        ClothingAttributes.DurabilityBonus = 2;
        Attributes.BonusDex = 10;
		SkillBonuses.SetValues(0, SkillName.Focus, 40.0);
        SkillBonuses.SetValues(0, SkillName.ItemID, 40.0); // Note: SkillName.ItemID is a placeholder; adjust if necessary
        Resistances.Cold = 10;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CuratorsKilt(Serial serial) : base(serial)
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
