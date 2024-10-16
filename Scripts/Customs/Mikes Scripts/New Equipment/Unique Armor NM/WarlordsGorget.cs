using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarlordsGorget : PlateGorget
{
    [Constructable]
    public WarlordsGorget()
    {
        Name = "Warlord's Gorget";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusStr = 15;
        Attributes.RegenHits = 3;
        SkillBonuses.SetValues(0, SkillName.Swords, 45.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarlordsGorget(Serial serial) : base(serial)
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
