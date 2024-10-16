using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThundergodsVigor : PlateGorget
{
    [Constructable]
    public ThundergodsVigor()
    {
        Name = "Thundergod's Vigor";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(35, 65);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.DurabilityBonus = 15;
        Attributes.BonusStr = 15;
        Attributes.ReflectPhysical = 5;
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThundergodsVigor(Serial serial) : base(serial)
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
