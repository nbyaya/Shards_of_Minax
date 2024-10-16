using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ValkyriesWard : MetalKiteShield
{
    [Constructable]
    public ValkyriesWard()
    {
        Name = "Valkyrie's Ward";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusHits = 40;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        SkillBonuses.SetValues(1, SkillName.Bushido, 20.0);
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ValkyriesWard(Serial serial) : base(serial)
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
