using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardiansHeartplate : PlateChest
{
    [Constructable]
    public GuardiansHeartplate()
    {
        Name = "Guardian's Heartplate";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 80;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 25.0);
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardiansHeartplate(Serial serial) : base(serial)
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
