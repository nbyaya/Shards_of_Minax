using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtiersEnchantedAmulet : PlateGorget
{
    [Constructable]
    public CourtiersEnchantedAmulet()
    {
        Name = "Courtier's Enchanted Amulet";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(35, 62);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 10;
        Attributes.SpellDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtiersEnchantedAmulet(Serial serial) : base(serial)
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
