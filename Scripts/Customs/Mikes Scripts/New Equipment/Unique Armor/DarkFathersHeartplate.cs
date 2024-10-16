using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkFathersHeartplate : PlateChest
{
    [Constructable]
    public DarkFathersHeartplate()
    {
        Name = "Dark Father's Heartplate";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusHits = 100;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 25;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkFathersHeartplate(Serial serial) : base(serial)
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
