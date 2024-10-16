using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperersBoots : Boots
{
    [Constructable]
    public WhisperersBoots()
    {
        Name = "Whisperer's Boots";
        Hue = Utility.Random(750, 1750);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperersBoots(Serial serial) : base(serial)
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
