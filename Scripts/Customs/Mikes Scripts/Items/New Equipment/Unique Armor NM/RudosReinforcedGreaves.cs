using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RudosReinforcedGreaves : PlateLegs
{
    [Constructable]
    public RudosReinforcedGreaves()
    {
        Name = "Rudo's Reinforced Greaves";
        Hue = Utility.Random(300, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 30;
        ArmorAttributes.SelfRepair = 15;
        Attributes.BonusStam = 35;
        Attributes.BonusStr = 25;
        Attributes.IncreasedKarmaLoss = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 45.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RudosReinforcedGreaves(Serial serial) : base(serial)
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
