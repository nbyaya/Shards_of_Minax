using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlackMagesMysticRobe : LeatherChest
{
    [Constructable]
    public BlackMagesMysticRobe()
    {
        Name = "BlackMage's Mystic Robe";
        Hue = Utility.Random(100, 600);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlackMagesMysticRobe(Serial serial) : base(serial)
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
