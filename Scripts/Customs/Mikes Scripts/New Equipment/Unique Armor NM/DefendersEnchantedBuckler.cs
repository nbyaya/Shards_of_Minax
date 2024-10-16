using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DefendersEnchantedBuckler : Buckler
{
    [Constructable]
    public DefendersEnchantedBuckler()
    {
        Name = "Defender's Enchanted Buckler";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.CastingFocus = 10;
        ArmorAttributes.SelfRepair = 20;
        Attributes.AttackChance = 15;
        Attributes.BonusStr = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 50.0);
        SkillBonuses.SetValues(1, SkillName.Wrestling, 30.0);
        PhysicalBonus = 25;
        EnergyBonus = 10;
        FireBonus = 20;
        ColdBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DefendersEnchantedBuckler(Serial serial) : base(serial)
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
