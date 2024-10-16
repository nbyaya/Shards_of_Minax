using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarlordsChestguard : PlateChest
{
    [Constructable]
    public WarlordsChestguard()
    {
        Name = "Warlord's Chestguard";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 100);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 25;
        Attributes.BonusHits = 30;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 15;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarlordsChestguard(Serial serial) : base(serial)
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
