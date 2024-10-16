using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsShield : MetalKiteShield
{
    [Constructable]
    public HammerlordsShield()
    {
        Name = "Hammerlord's Shield";
        Hue = Utility.Random(350, 650);
        BaseArmorRating = Utility.RandomMinMax(30, 70);
        AbsorptionAttributes.EaterDamage = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 10;
        Attributes.ReflectPhysical = 5;
        SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
        PhysicalBonus = 25;
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsShield(Serial serial) : base(serial)
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
