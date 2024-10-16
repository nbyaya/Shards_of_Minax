using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsBulwark : HeaterShield
{
    [Constructable]
    public DragonsBulwark()
    {
        Name = "Dragon's Bulwark";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 25;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        EnergyBonus = 10;
        FireBonus = 25;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsBulwark(Serial serial) : base(serial)
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
