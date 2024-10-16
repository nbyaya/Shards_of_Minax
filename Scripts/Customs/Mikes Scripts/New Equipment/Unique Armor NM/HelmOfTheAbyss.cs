using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HelmOfTheAbyss : PlateHelm
{
    [Constructable]
    public HelmOfTheAbyss()
    {
        Name = "Helm of the Abyss";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 75);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.AttackChance = 15;
        Attributes.BonusHits = 40;
        Attributes.DefendChance = 15;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Tactics, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HelmOfTheAbyss(Serial serial) : base(serial)
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
