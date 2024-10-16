using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowlurkersChestguard : LeatherChest
{
    [Constructable]
    public ShadowlurkersChestguard()
    {
        Name = "Shadowlurker's Chestguard";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        ArmorAttributes.MageArmor = 1;
        Attributes.EnhancePotions = 25;
        Attributes.RegenStam = 5;
        Attributes.BonusStr = 20;
        SkillBonuses.SetValues(0, SkillName.Hiding, 50.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 40.0);
        SkillBonuses.SetValues(2, SkillName.Stealing, 40.0);
        PhysicalBonus = 20;
        ColdBonus = 20;
        FireBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowlurkersChestguard(Serial serial) : base(serial)
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
