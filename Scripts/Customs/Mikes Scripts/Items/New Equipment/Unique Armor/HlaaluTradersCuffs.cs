using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HlaaluTradersCuffs : RingmailArms
{
    [Constructable]
    public HlaaluTradersCuffs()
    {
        Name = "Hlaalu Trader's Cuffs";
        Hue = Utility.Random(150, 400);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusDex = 25;
        Attributes.LowerRegCost = 10;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HlaaluTradersCuffs(Serial serial) : base(serial)
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
