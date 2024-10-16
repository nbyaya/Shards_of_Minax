using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhiteRidersGuard : WoodenShield
{
    [Constructable]
    public WhiteRidersGuard()
    {
        Name = "White Rider's Guard";
        Hue = Utility.Random(400, 600);
        BaseArmorRating = Utility.RandomMinMax(52, 82);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.ReflectPhysical = 15;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Meditation, 25.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhiteRidersGuard(Serial serial) : base(serial)
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
