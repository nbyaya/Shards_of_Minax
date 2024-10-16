using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CovensShadowedHood : BoneHelm
{
    [Constructable]
    public CovensShadowedHood()
    {
        Name = "Coven's Shadowed Hood";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 65);
        AbsorptionAttributes.EaterPoison = 25;
        ArmorAttributes.SelfRepair = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CovensShadowedHood(Serial serial) : base(serial)
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
