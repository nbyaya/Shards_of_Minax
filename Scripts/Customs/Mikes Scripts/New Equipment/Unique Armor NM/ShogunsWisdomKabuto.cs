using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShogunsWisdomKabuto : PlateHelm
{
    [Constructable]
    public ShogunsWisdomKabuto()
    {
        Name = "Shogun's Wisdom Kabuto";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Bushido, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 20;
        FireBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShogunsWisdomKabuto(Serial serial) : base(serial)
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
