using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SorceressMidnightRobe : Robe
{
    [Constructable]
    public SorceressMidnightRobe()
    {
        Name = "Sorceress's Midnight Robe";
        Hue = Utility.Random(400, 2500);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusInt = 20;
        Attributes.CastSpeed = 1;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SorceressMidnightRobe(Serial serial) : base(serial)
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
