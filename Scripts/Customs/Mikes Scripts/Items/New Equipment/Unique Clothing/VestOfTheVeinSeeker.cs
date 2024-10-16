using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VestOfTheVeinSeeker : Tunic
{
    [Constructable]
    public VestOfTheVeinSeeker()
    {
        Name = "Vest of the Vein Seeker";
        Hue = Utility.Random(300, 1300);
        ClothingAttributes.DurabilityBonus = 3;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Mining, 25.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 10.0); // Assuming ItemID is a placeholder for a specific skill
        Resistances.Cold = 10;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VestOfTheVeinSeeker(Serial serial) : base(serial)
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
