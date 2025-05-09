using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShanachiesStorytellingShoes : Shoes
{
    [Constructable]
    public ShanachiesStorytellingShoes()
    {
        Name = "Shanachie's Storytelling Shoes";
        Hue = Utility.Random(400, 2430);
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);
        Resistances.Cold = 5;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShanachiesStorytellingShoes(Serial serial) : base(serial)
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
