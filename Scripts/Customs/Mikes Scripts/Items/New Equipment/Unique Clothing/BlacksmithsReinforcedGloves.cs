using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlacksmithsReinforcedGloves : Bandana
{
    [Constructable]
    public BlacksmithsReinforcedGloves()
    {
        Name = "Blacksmith's Reinforced Gloves";
        Hue = Utility.Random(500, 1500);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusDex = 15;
        Attributes.WeaponDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        Resistances.Fire = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlacksmithsReinforcedGloves(Serial serial) : base(serial)
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
