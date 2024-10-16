using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CraftsmansProtectiveGloves : Bandana
{
    [Constructable]
    public CraftsmansProtectiveGloves()
    {
        Name = "Craftsman's Protective Gloves";
        Hue = Utility.Random(720, 1670);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 15.0);
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 10.0);
        Resistances.Physical = 15;
        Resistances.Poison = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CraftsmansProtectiveGloves(Serial serial) : base(serial)
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
