using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BootsOfTheDeepCaverns : Boots
{
    [Constructable]
    public BootsOfTheDeepCaverns()
    {
        Name = "Boots of the Deep Caverns";
        Hue = Utility.Random(500, 1500);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusDex = 7;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        Resistances.Physical = 20;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BootsOfTheDeepCaverns(Serial serial) : base(serial)
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
