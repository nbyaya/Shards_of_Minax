using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DaimyosHonorTekko : PlateGloves
{
    [Constructable]
    public DaimyosHonorTekko()
    {
        Name = "Daimyo's Honor Tekko";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 15;
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.SelfRepair = 10;
        SkillBonuses.SetValues(0, SkillName.Bushido, 40.0);
        SkillBonuses.SetValues(1, SkillName.Wrestling, 30.0);
        PhysicalBonus = 17;
        FireBonus = 17;
        ColdBonus = 17;
        EnergyBonus = 17;
        PoisonBonus = 17;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DaimyosHonorTekko(Serial serial) : base(serial)
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
