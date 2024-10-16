using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MondainsSkull : BoneHelm
{
    [Constructable]
    public MondainsSkull()
    {
        Name = "Mondain's Skull";
        Hue = Utility.Random(666, 777);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        AbsorptionAttributes.EaterEnergy = 20;
        Attributes.ReflectPhysical = 10;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MondainsSkull(Serial serial) : base(serial)
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
