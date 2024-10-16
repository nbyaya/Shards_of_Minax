using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrusadersHelmet : PlateHelm
{
    [Constructable]
    public CrusadersHelmet()
    {
        Name = "Crusader's Helmet";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStr = 10;
        Attributes.BonusHits = 5;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 10.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 5.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 5.0);
        SkillBonuses.SetValues(3, SkillName.Tactics, 5.0);
        PhysicalBonus = 12;
        FireBonus = 5;
        ColdBonus = 5;
        EnergyBonus = 5;
        PoisonBonus = 13;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrusadersHelmet(Serial serial) : base(serial)
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
