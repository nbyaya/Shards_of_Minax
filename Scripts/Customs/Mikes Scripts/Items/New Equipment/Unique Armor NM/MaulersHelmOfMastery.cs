using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaulersHelmOfMastery : PlateHelm
{
    [Constructable]
    public MaulersHelmOfMastery()
    {
        Name = "Mauler's Helm of Mastery";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        ArmorAttributes.LowerStatReq = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStr = 15;
        Attributes.RegenHits = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 40.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaulersHelmOfMastery(Serial serial) : base(serial)
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
