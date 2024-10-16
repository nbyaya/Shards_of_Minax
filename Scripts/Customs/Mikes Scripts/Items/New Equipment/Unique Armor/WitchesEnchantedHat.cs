using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesEnchantedHat : LeatherCap
{
    [Constructable]
    public WitchesEnchantedHat()
    {
        Name = "Witch's Enchanted Hat";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 15;
        Attributes.CastRecovery = 1;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesEnchantedHat(Serial serial) : base(serial)
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
