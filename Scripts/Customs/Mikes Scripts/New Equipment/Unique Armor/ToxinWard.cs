using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ToxinWard : PlateArms
{
    [Constructable]
    public ToxinWard()
    {
        Name = "Toxin Ward";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(35, 75);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ToxinWard(Serial serial) : base(serial)
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
