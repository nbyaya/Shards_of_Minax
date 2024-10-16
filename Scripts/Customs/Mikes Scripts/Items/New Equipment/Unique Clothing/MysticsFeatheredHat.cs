using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticsFeatheredHat : FeatheredHat
{
    [Constructable]
    public MysticsFeatheredHat()
    {
        Name = "Mystic's Feathered Hat";
        Hue = Utility.Random(1, 1000);
        Attributes.BonusInt = 15;
        Attributes.EnhancePotions = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
        Resistances.Energy = 20;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticsFeatheredHat(Serial serial) : base(serial)
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
