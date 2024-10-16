using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarlordsEmbraceYodareKake : PlateGorget
{
    [Constructable]
    public WarlordsEmbraceYodareKake()
    {
        Name = "Warlord's Embrace Yodare-Kake";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 20;
        ArmorAttributes.DurabilityBonus = 75;
        SkillBonuses.SetValues(0, SkillName.Bushido, 45.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 20;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarlordsEmbraceYodareKake(Serial serial) : base(serial)
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
