using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GladiatorsHelmet : PlateHelm
{
    [Constructable]
    public GladiatorsHelmet()
    {
        Name = "Gladiator's Helmet";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStr = 20;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 25;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GladiatorsHelmet(Serial serial) : base(serial)
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
