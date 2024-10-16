using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesBewitchingRobe : Robe
{
    [Constructable]
    public WitchesBewitchingRobe()
    {
        Name = "Witch's Bewitching Robe";
        Hue = Utility.Random(1, 2500);
        ClothingAttributes.SelfRepair = 5;
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Alchemy, 15.0);
        Resistances.Energy = 20;
        Resistances.Poison = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesBewitchingRobe(Serial serial) : base(serial)
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
