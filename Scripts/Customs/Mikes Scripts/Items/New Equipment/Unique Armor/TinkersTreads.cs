using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TinkersTreads : PlateLegs
{
    [Constructable]
    public TinkersTreads()
    {
        Name = "Tinker's Treads";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(45, 88);
        AbsorptionAttributes.EaterKinetic = 25;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.AttackChance = 15;
        Attributes.WeaponDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 30.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TinkersTreads(Serial serial) : base(serial)
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
