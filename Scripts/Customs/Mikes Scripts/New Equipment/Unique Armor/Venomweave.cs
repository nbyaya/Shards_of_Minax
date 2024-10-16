using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Venomweave : LeatherLegs
{
    [Constructable]
    public Venomweave()
    {
        Name = "Venomweave";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 70);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 20;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Venomweave(Serial serial) : base(serial)
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
