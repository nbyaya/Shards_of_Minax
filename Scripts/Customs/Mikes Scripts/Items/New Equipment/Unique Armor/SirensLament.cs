using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SirensLament : WoodenKiteShield
{
    [Constructable]
    public SirensLament()
    {
        Name = "Siren's Lament";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 20;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Musicianship, 15.0);
        ColdBonus = 25;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SirensLament(Serial serial) : base(serial)
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
