using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrillmastersChainLeggings : ChainLegs
{
    [Constructable]
    public GrillmastersChainLeggings()
    {
        Name = "Grillmaster's Chain Leggings";
        Hue = 1137;
        BaseArmorRating = Utility.RandomMinMax(30, 40);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusStam = 25;
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Cooking, 50.0);
        SkillBonuses.SetValues(1, SkillName.Fishing, 30.0);
        SkillBonuses.SetValues(2, SkillName.Carpentry, 30.0);
        PhysicalBonus = 20;
        FireBonus = 20;
        ColdBonus = 10;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrillmastersChainLeggings(Serial serial) : base(serial)
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
