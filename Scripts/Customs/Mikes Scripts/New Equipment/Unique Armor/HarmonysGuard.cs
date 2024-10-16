using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonysGuard : WoodenKiteShield
{
    [Constructable]
    public HarmonysGuard()
    {
        Name = "Harmony's Guard";
        Hue = Utility.Random(150, 550);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 10;
        Attributes.ReflectPhysical = 5;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonysGuard(Serial serial) : base(serial)
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
