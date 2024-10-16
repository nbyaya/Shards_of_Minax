using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SamuraisHonorableTunic : Tunic
{
    [Constructable]
    public SamuraisHonorableTunic()
    {
        Name = "Samurai's Honorable Tunic";
        Hue = Utility.Random(700, 2700);
        ClothingAttributes.DurabilityBonus = 5;
        Attributes.BonusStr = 15;
        SkillBonuses.SetValues(0, SkillName.Bushido, 25.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SamuraisHonorableTunic(Serial serial) : base(serial)
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
