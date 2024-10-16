using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlackMagesRuneRobe : LeatherChest
{
    [Constructable]
    public BlackMagesRuneRobe()
    {
        Name = "BlackMage's Rune Robe";
        Hue = Utility.Random(50, 250);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 25;
        Attributes.SpellDamage = 10;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlackMagesRuneRobe(Serial serial) : base(serial)
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
