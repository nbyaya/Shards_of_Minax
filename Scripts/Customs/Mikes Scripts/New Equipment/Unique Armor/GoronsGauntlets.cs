using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GoronsGauntlets : PlateGloves
{
    [Constructable]
    public GoronsGauntlets()
    {
        Name = "Goron's Gauntlets";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStr = 35;
        Attributes.WeaponDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GoronsGauntlets(Serial serial) : base(serial)
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
