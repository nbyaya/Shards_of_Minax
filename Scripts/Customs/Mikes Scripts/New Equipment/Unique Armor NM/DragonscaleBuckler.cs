using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonscaleBuckler : Buckler
{
    [Constructable]
    public DragonscaleBuckler()
    {
        Name = "Dragonscale Buckler";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(25, 50);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.SelfRepair = 5;
        Attributes.ReflectPhysical = 5;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        PhysicalBonus = 15;
        FireBonus = 25;
        EnergyBonus = 10;
        ColdBonus = 0;
        PoisonBonus = 0;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonscaleBuckler(Serial serial) : base(serial)
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
