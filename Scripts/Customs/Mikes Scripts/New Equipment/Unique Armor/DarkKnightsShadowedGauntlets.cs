using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkKnightsShadowedGauntlets : PlateGloves
{
    [Constructable]
    public DarkKnightsShadowedGauntlets()
    {
        Name = "Dark Knight's Shadowed Gauntlets";
        Hue = Utility.Random(10, 20);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusDex = 40;
        Attributes.AttackChance = 25;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 25;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkKnightsShadowedGauntlets(Serial serial) : base(serial)
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
