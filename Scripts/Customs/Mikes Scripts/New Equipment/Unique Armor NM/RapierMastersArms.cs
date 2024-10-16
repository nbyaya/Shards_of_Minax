using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RapierMastersArms : LeatherArms
{
    [Constructable]
    public RapierMastersArms()
    {
        Name = "Rapier Master's Arms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 60);
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusDex = 40;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Fencing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 40.0);
        PhysicalBonus = 18;
        ColdBonus = 8;
        FireBonus = 8;
        EnergyBonus = 16;
        PoisonBonus = 8;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RapierMastersArms(Serial serial) : base(serial)
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
