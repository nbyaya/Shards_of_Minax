using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class OrkArdHat : BoneHelm
{
    [Constructable]
    public OrkArdHat()
    {
        Name = "Ork 'Ard Hat";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.DurabilityBonus = 30;
        Attributes.BonusHits = 30;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 25;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public OrkArdHat(Serial serial) : base(serial)
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
