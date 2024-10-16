using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AshlandersResilience : ChainChest
{
    [Constructable]
    public AshlandersResilience()
    {
        Name = "Ashlander's Resilience";
        Hue = Utility.Random(10, 510);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStam = 20;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AshlandersResilience(Serial serial) : base(serial)
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
